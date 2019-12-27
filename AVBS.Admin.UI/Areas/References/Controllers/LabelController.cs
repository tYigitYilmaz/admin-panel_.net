using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Admin.UI.Controllers.Base;
using AVBS.Business.Abstract;
using AVBS.Entity.References;
using AVBS.Infrastructure;
using AVBS.View.Concrete.Table;
using AVBS.Web.UI.App_Data;

namespace AVBS.Admin.UI.Areas.References.Controllers {
    [Authentication]
    public class LabelController : IController< AVBS_Label, BaseBusiness<AVBS_Label>> {
         

        public override void ArrangeIndexViewBag () { 
             
        }

        public override void ArrangeViewBag ( int id = 0 ) { 
        }

        // TODO model bazlı kontrol yapılacak
        public override void EditSpecifications ( int id = 0 ) {

        }

        public override void CreateSpecifications ( int id = 0 ) {

        }

        public override void ModelOperations ( ref AVBS_Label model ) {

        }

        public JsonResult GetList (  JQueryDataTableParam param, int locationid = 0 ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var cashBookBusiness = new BaseBusiness<AVBS_Label>();
                var allData = cashBookBusiness.GetAllQueryable(   );

                // Search
                if ( !string.IsNullOrEmpty( searchVal ) ) {
                    var searchQuery = searchVal.ToLower();
                    allData = allData.Where( x =>
                                            x.Name.ToLower().Contains( searchQuery ) 
                    );
                }

                // Configure Order
                var orderingFunction = new Func<AVBS_Label, object>( c => c.Id );
                if ( orderColumn == 0 ) {
                    orderingFunction = ( x => x.Name );
                } 

                // Order and arrange displayed data according to skip, take parameters
                var displayedData = orderDirection == "asc" ? allData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).ToList() : allData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).ToList();

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