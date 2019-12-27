using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace AVBS.Infrastructure {
    /// <summary>
    /// Parses the request values from a query from the DataTables jQuery pluggin
    /// </summary>
    /// <typeparam name="T">List data type</typeparam>
    public class DataTablesParser<T> where T : class, new() {

        private IQueryable<T> _queriable;
        private readonly HttpRequestBase _httpRequest;
        private readonly String[] _searcList;
        private readonly Type _type;
        private IDictionary<int, PropertyMapping> _propertyMap;


        //TODO: We may be able to handle other numeric property types if they are translatable
        private Type[] _translatable =
        {
            typeof(string),
            typeof(int),
            typeof(Nullable<int>),
            typeof(decimal),
            typeof(Nullable<decimal>),
            typeof(float),
            typeof(Nullable<float>),
            typeof(DateTime),
            typeof(Nullable<DateTime>),
            typeof(long),
            typeof(Nullable<long>)

        };

        public DataTablesParser ( HttpRequestBase httpRequest, IQueryable<T> queriable, String[] searcList ) {
            _queriable = queriable;
            _httpRequest = httpRequest;
            _type = typeof( T );

            _searcList = searcList;
            //This associates class properties with relevant datatable configuration options
            //Single pass for key then hash lookups for corresponding properties 
            _propertyMap = ( from key in _httpRequest.Params.AllKeys.Where( k => Regex.IsMatch( k, Constants.COLUMN_PROPERTY_PATTERN ) )
                             join prop in _type.GetProperties() on _httpRequest[key] equals prop.Name
                             let index = Regex.Match( key, Constants.COLUMN_PROPERTY_PATTERN ).Groups[1].Value
                             let searchableKey = Constants.GetKey( Constants.SEARCHABLE_PROPERTY_FORMAT, index )
                             let searchable = _httpRequest[searchableKey] == null ? true : _httpRequest[searchableKey].Trim() == "true"
                             let orderableKey = Constants.GetKey( Constants.ORDERABLE_PROPERTY_FORMAT, index )
                             let orderable = _httpRequest[orderableKey] == null ? true : _httpRequest[orderableKey].Trim() == "true"
                             // Set regex and individual search when implemented

                             select new {
                                 index = int.Parse( index ),
                                 map = new PropertyMapping {
                                     Property = prop,
                                     Searchable = searchable,
                                     Orderable = orderable
                                 }
                             } ).Distinct().ToDictionary( k => k.index, v => v.map );
        }

        public DataTablesParser ( HttpRequest httpRequest, IQueryable<T> queriable, String[] searcList )
            : this( new HttpRequestWrapper( httpRequest ), queriable, searcList ) { }

        /// <summary>
        /// Parses the <see cref="HttpRequestBase"/> parameter values for the accepted 
        /// DataTable request values
        /// </summary>
        /// <returns>Formated output for DataTables, which should be serialized to JSON</returns>

        public FormatedList<T> Parse () {
            var list = new FormatedList<T>();

            // parse the echo property (must be returned as int to prevent XSS-attack)
            list.draw = int.Parse( _httpRequest[Constants.DRAW] );

            // count the record BEFORE filtering
            list.recordsTotal = _queriable.Count();

            // apply the sort, if there is one
            ApplySort();

            // parse the paging values
            int skip = 0, take = 10;
            int.TryParse( _httpRequest[Constants.DISPLAY_START], out skip );
            int.TryParse( _httpRequest[Constants.DISPLAY_LENGTH], out take );

            //This needs to be an expression or else it won't limit results
            Func<T, bool> GenericFind = delegate ( T item ) {
                bool found = false;
                var sSearch = _httpRequest[Constants.SEARCH_KEY];

                if ( string.IsNullOrWhiteSpace( sSearch ) ) {
                    return true;
                }

                foreach ( var map in _propertyMap ) {

                    if ( map.Value.Searchable && Convert.ToString( map.Value.Property.GetValue( item, null ) ).ToLower().Contains( ( sSearch ).ToLower() ) ) {
                        found = true;
                    }
                }
                return found;

            };

            //Test for linq to entities
            //Anyone know of a better way to do this test??
            if ( _queriable is ObjectQuery<T> || _queriable is DbQuery<T> ) {

                // setup the data with individual property search, all fields search,
                // paging, and property list selection
                var resultQuery = _queriable.Where( ApplyGenericSearch )
                            .Skip( skip )
                            .Take( take );

                list.data = resultQuery.ToList();

                list.SetQuery( resultQuery.ToString() );

                // total records that are displayed after filter
                list.recordsFiltered = _queriable.Count( ApplyGenericSearch );
            } else //linq to objects
              {

                // setup the data with individual property search, all fields search,
                // paging, and property list selection
                var resultQuery = _queriable.Where( GenericFind )
                            .Skip( skip )
                            .Take( take );

                list.data = resultQuery
                            .ToList();

                list.SetQuery( resultQuery.ToString() );


                // total records that are displayed after filter
                list.recordsFiltered = string.IsNullOrWhiteSpace( _httpRequest[Constants.SEARCH_KEY] ) ? list.recordsTotal : _queriable.Count( GenericFind );
            }



            return list;
        }

        public async Task<FormatedList<T>> ParseAsync () {
            return await Task.Run( () => {
                return Parse();
            } );

        }


        private void ApplySort () {
            var sorted = false;
            var paramExpr = Expression.Parameter( typeof( T ), "val" );
            Expression propertyExpr;
            Type propType = null;
            // Enumerate the keys sort keys in the order we received them
            foreach ( string key in _httpRequest.Params.AllKeys.Where( x => Regex.IsMatch( x, Constants.ORDER_PATTERN ) ) ) {
                // column number to sort (same as the array)


                var column = _httpRequest[key];
                var columnKey = Constants.GetKey( Constants.DATA_PROPERTY_FORMAT, column );
                string columnName = _httpRequest[columnKey];
                int sortcolumn = int.Parse( _httpRequest[key] );
                var index = Regex.Match( key, Constants.ORDER_PATTERN ).Groups[1].Value;
                var orderDirectionKey = Constants.GetKey( Constants.ORDER_DIRECTION_FORMAT, index );
                string sortdir = _httpRequest[orderDirectionKey];
                if ( columnName.Contains( "." ) ) {

                    var expression1 = columnName.Split( '.' )
                           .Aggregate( paramExpr,
                               ( Expression parent, string path ) => Expression.Property( parent, path ) );
                    propType = expression1.Type;
                    var delegateType = Expression.GetFuncType( typeof( T ), propType );
                    propertyExpr = Expression.Lambda( delegateType, expression1, paramExpr );

                } else {
                    // ignore invalid for disabled columns 
                    if ( !_propertyMap.ContainsKey( sortcolumn ) || !_propertyMap[sortcolumn].Orderable )
                        continue;


                    // get the direction of the sort


                    var sortProperty = _propertyMap[sortcolumn].Property;
                    var expression1 = Expression.Property( paramExpr, sortProperty );
                    propType = sortProperty.PropertyType;
                    var delegateType = Expression.GetFuncType( typeof( T ), propType );
                    propertyExpr = Expression.Lambda( delegateType, expression1, paramExpr );
                }
                // apply the sort (default is ascending if not specified)
                string methodName;
                if ( string.IsNullOrEmpty( sortdir ) || sortdir.Equals( Constants.ASCENDING_SORT, StringComparison.OrdinalIgnoreCase ) ) {
                    methodName = sorted ? "ThenBy" : "OrderBy";
                } else {
                    methodName = sorted ? "ThenByDescending" : "OrderByDescending";
                }

                _queriable = typeof( Queryable ).GetMethods().Single(
                    method => method.Name == methodName
                                && method.IsGenericMethodDefinition
                                && method.GetGenericArguments().Length == 2
                                && method.GetParameters().Length == 2 )
                        .MakeGenericMethod( typeof( T ), propType )
                        .Invoke( null, new object[] { _queriable, propertyExpr } ) as IOrderedQueryable<T>;

                sorted = true;
            }

            //Linq to entities needs a sort to implement skip
            //Not sure if we care about the queriables that come in sorted? IOrderedQueryable does not seem to be a reliable test
            if ( !sorted ) {
                var firstProp = Expression.Property( paramExpr, _propertyMap.First().Value.Property );
                var propTypes = _propertyMap.First().Value.Property.PropertyType;
                var delegateType = Expression.GetFuncType( typeof( T ), propTypes );
                var propertyExprs = Expression.Lambda( delegateType, firstProp, paramExpr );

                _queriable = typeof( Queryable ).GetMethods().Single(
             method => method.Name == "OrderBy"
                         && method.IsGenericMethodDefinition
                         && method.GetGenericArguments().Length == 2
                         && method.GetParameters().Length == 2 )
                 .MakeGenericMethod( typeof( T ), propTypes )
                 .Invoke( null, new object[] { _queriable, propertyExprs } ) as IOrderedQueryable<T>;

            }

        }



        /// <summary>
        /// Expression for an all column search, which will filter the result based on this criterion
        /// </summary>
        private Expression<Func<T, bool>> ApplyGenericSearch {
            get {


                // invariant expressions
                Expression compoundExpression = null;
                var paramExpression = Expression.Parameter( typeof( T ), "val" );
                var conversionLength = Expression.Constant( 10, typeof( Nullable<int> ) );
                var conversionDecimals = Expression.Constant( 16, typeof( Nullable<int> ) );
                List<MethodCallExpression> searchProps = new List<MethodCallExpression>();

                T entity = new T();
                foreach ( var item in _searcList ) {
                    string s = _httpRequest[item];

                    if ( string.IsNullOrWhiteSpace( s ) ) {
                        if ( string.IsNullOrWhiteSpace( _httpRequest[item + "2"] ) )
                            continue;
                        else {
                            s = _httpRequest[item + "2"];
                        }
                    }

                    var searchExpression = Expression.Constant( s );
                    Type property = null;
                    Expression propExp = null;
                    if ( item.Contains( "." ) ) {
                        propExp = item.Split( '.' )
                            .Aggregate( paramExpression,
                                ( Expression parent, string path ) => Expression.Property( parent, path ) );
                        property = propExp.Type;
                    } else {
                        propExp = Expression.Property( paramExpression, item );
                        property = entity.GetType().GetProperty( item ).PropertyType;
                    }

                    Expression stringProp = null; //The result must be a TSQL translatable string expression



                    //TODO: find some genius way to categorize numeric properties including their nullable<> variants
                    if ( new Type[] { typeof( int ), typeof( Nullable<int> ), typeof( double ), typeof( Nullable<double> ), typeof( float ), typeof( Nullable<float> ), typeof( long ), typeof( Nullable<long> ) }.Contains( property ) ) {
                        searchExpression = ConvertType( _httpRequest[item], property );
                        Expression prop = Expression.Convert( propExp, property );
                        compoundExpression = AddQuery( compoundExpression, Expression.Equal( prop, searchExpression ) );
                    }

                    if ( property == typeof( decimal ) || property == typeof( Nullable<decimal> ) ) {
                        var toDoubleCall = Expression.Convert( propExp, typeof( Nullable<decimal> ) );

                        var doubleConvert = typeof( SqlFunctions ).GetMethod( "StringConvert", new Type[] { typeof( Nullable<decimal> ), typeof( Nullable<int> ), typeof( Nullable<int> ) } );

                        stringProp = Expression.Call( doubleConvert, toDoubleCall, conversionLength, conversionDecimals );
                        compoundExpression = AddQuery( compoundExpression, Expression.Call( stringProp, typeof( string ).GetMethod( "Contains" ), searchExpression ) );
                    }

                    //TODO: Provide a way to customize date format
                    else if ( ( property == typeof( DateTime ) || property == typeof( Nullable<DateTime> ) ) ) {
                        if ( IsDate( _httpRequest[item] ) ) {
                            searchExpression = Expression.Constant( DateTime.Parse( _httpRequest[item] ) );
                            Expression date1 = Expression.GreaterThanOrEqual( propExp, searchExpression );
                            compoundExpression = AddQuery( compoundExpression, date1 );
                        }
                        string fromto = _httpRequest[item + "2"];
                        if ( !string.IsNullOrWhiteSpace( fromto ) && IsDate( fromto ) ) {
                            searchExpression = Expression.Constant( DateTime.Parse( fromto ) );
                            Expression date2 = Expression.LessThanOrEqual( propExp, searchExpression );
                            compoundExpression = AddQuery( compoundExpression, date2 );
                        }
                    } else if ( property == typeof( string ) ) {
                        stringProp = propExp;

                        compoundExpression = AddQuery( compoundExpression, Expression.Call( stringProp, typeof( string ).GetMethod( "Contains" ), searchExpression ) );
                    } else if ( property == typeof( bool ) ) {
                        searchExpression = ConvertType( _httpRequest[item], property );
                        Expression prop = Expression.Convert( propExp, property );
                        compoundExpression = AddQuery( compoundExpression, Expression.Equal( prop, searchExpression ) );
                    }

                }

                // we now need to compound the expression by starting with the first
                if ( compoundExpression == null )
                    return x => true;


                // compile the expression into a lambda 
                return Expression.Lambda<Func<T, bool>>( compoundExpression, paramExpression );
            }
        }

        private ConstantExpression ConvertType ( string value, Type type ) {
            if ( type == typeof( double ) ) {
                return Expression.Constant( double.Parse( value ), type );
            } else if ( type == typeof( float ) ) {
                return Expression.Constant( float.Parse( value ), type );

            } else if ( type == typeof( long ) ) {
                return Expression.Constant( long.Parse( value ), type );
            } else if ( type == typeof( bool ) ) {
                return Expression.Constant( bool.Parse( value ), type );
            } else {
                return Expression.Constant( int.Parse( value ), type );
            }

        }

        private Expression AddQuery ( Expression compoundExpression, Expression query ) {
            if ( compoundExpression == null ) {
                compoundExpression = query;

            } else {
                compoundExpression = Expression.And( compoundExpression, query );
            }
            return compoundExpression;
        }

        private bool IsDate ( string date ) {
            DateTime dateTime;
            if ( DateTime.TryParse( date, out dateTime ) ) {
                return true;
            }
            return false;
        }



        private class PropertyMapping {
            public PropertyInfo Property { get; set; }
            public bool Orderable { get; set; }
            public bool Searchable { get; set; }
            public string Regex { get; set; } //Not yet implemented
            public string SearchInput { get; set; } //Not yet implemented
        }
    }

    public class FormatedList<T> {
        private string _query;

        internal void SetQuery ( string query ) {
            _query = query;
        }

        public string GetQuery () {
            return _query;
        }

        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }

    }

    public class Constants {
        public const string COLUMN_PROPERTY_PATTERN = @"columns\[(\d+)\]\[data\]";
        public const string ORDER_PATTERN = @"order\[(\d+)\]\[column\]";

        public const string DISPLAY_START = "start";
        public const string DISPLAY_LENGTH = "length";
        public const string DRAW = "draw";
        public const string ASCENDING_SORT = "asc";
        public const string SEARCH_KEY = "search[value]";
        public const string SEARCH_REGEX_KEY = "search[regex]";

        public const string DATA_PROPERTY_FORMAT = "columns[{0}][data]";
        public const string SEARCHABLE_PROPERTY_FORMAT = "columns[{0}][searchable]";
        public const string ORDERABLE_PROPERTY_FORMAT = "columns[{0}][orderable]";
        public const string SEARCH_VALUE_PROPERTY_FORMAT = "columns[{0}][search][value]";
        public const string SEARCH_REGEX_PROPERTY_FORMAT = "columns[{0}][search][regex]";
        public const string ORDER_COLUMN_FORMAT = "order[{0}][column]";
        public const string ORDER_DIRECTION_FORMAT = "order[{0}][dir]";

        public static string GetKey ( string format, string index ) {
            return String.Format( format, index );
        }
    }
}
