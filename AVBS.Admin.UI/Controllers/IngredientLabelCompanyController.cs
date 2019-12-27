using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Admin.UI.Controllers.Base; 
using AVBS.Business.Abstract;
using AVBS.Entity.Concrete;
using AVBS.Entity.References;
using AVBS.Infrastructure;
using AVBS.View.Concrete.Table;
using AVBS.Admin.UI.App_Data;
using AVBS.Business.Concrete;

namespace AVBS.Admin.UI.Controllers {
    [Authentication]
    public class IngredientLabelCompanyController : IController< AVBS_IngredientLabelCompany, BaseBusiness<AVBS_IngredientLabelCompany>> {
         

        public override void ArrangeIndexViewBag () { 
             
        }

        public override void ArrangeViewBag ( int id = 0 ) {

            var workWithCompanyService = new BaseBusiness<AVBS_WorkWithCompany>( );
            ViewBag.WorkWithCompanyList = workWithCompanyService.GetAllQueryable().Select( x => new { x.Id, x.Name } ).ToList();

            var ingredientService = new BaseBusiness<AVBS_Ingredient>( );
            ViewBag.IngredientList = ingredientService.GetAllQueryable().Select( x => new { x.Id, x.Name } ).ToList();

            var labelService = new BaseBusiness<AVBS_Label>( );
            ViewBag.LabelList = labelService.GetAllQueryable().Select( x => new { x.Id, x.Name } ).ToList();

            var unitTypeService = new BaseBusiness<AVBS_UnitType>( );
            ViewBag.UnitTypeList = unitTypeService.GetAllQueryable().Select( x => new { x.Id, x.Name } ).ToList();
        }

        // TODO model bazlı kontrol yapılacak
        public override void EditSpecifications ( int id = 0 ) {

        }

        public override void CreateSpecifications ( int id = 0 ) {

        }

        public override void ModelOperations ( ref AVBS_IngredientLabelCompany model ) {

        }

        public JsonResult GetList (  JQueryDataTableParam param ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var ingredientLabelCompanyService = new ViewBusiness<AVBS_IngredientLabelCompanyTableView>();
                var allData = ingredientLabelCompanyService.GetAllQueryable(   );

                // Search
                if ( !string.IsNullOrEmpty( searchVal ) ) {
                    var searchQuery = searchVal.ToLower();
                    allData = allData.Where( x =>
                                            x.IngredientName.ToLower().Contains( searchQuery ) ||
                                            x.LabelName.ToLower().Contains( searchQuery ) ||
                                            x.WorkWithCompanyName.ToLower().Contains( searchQuery )
                    );
                }

                // Configure Order
                var orderingFunction = new Func<AVBS_IngredientLabelCompanyTableView, object>( c => c.Id );
                if ( orderColumn == 0 ) {
                    orderingFunction = ( x => x.WorkWithCompanyName );
                } else if ( orderColumn == 1 ) {
                    orderingFunction = ( x => x.LabelName );
                } else if ( orderColumn == 2 ) {
                    orderingFunction = ( x => x.IngredientName );
                }

                // Order and arrange displayed data according to skip, take parameters
                var displayedData = orderDirection == "desc" ? allData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).ToList() : allData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).ToList();

                var totalNumber = allData.Count(); 

                return
                    Json(
                        new {
                            draw = param.draw,
                            recordsTotal = totalNumber,
                            recordsFiltered = totalNumber,
                            data = displayedData
                        }, JsonRequestBehavior.AllowGet );
            } catch ( Exception e ) {

                return null;
            }
        }
         
    }
}