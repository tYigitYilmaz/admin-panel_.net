using System;
using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Admin.UI.Controllers.Base; 
using AVBS.Business.Abstract;
using AVBS.Entity.Concrete;
using AVBS.Entity.References;
using AVBS.Infrastructure;
using AVBS.View.Concrete.SelectList;
using AVBS.View.Concrete.Table;
using AVBS.Admin.UI.App_Data;
using AVBS.Business.Concrete;

namespace AVBS.Admin.UI.Controllers {
    [Authentication]
    public class ProductPurchaseController : IController< AVBS_ProductPurchase, BaseBusiness<AVBS_ProductPurchase>> {
         

        public override void ArrangeIndexViewBag () {
            var monthTermService = new BaseBusiness<AVBS_MonthlyTerm>();
            var monthTermList = monthTermService.GetAllQueryable().Select( x => new { Id = x.Name, Name = x.Name } ).ToList();

            ViewBag.MonthTermList = monthTermList;
            ViewBag.CurrentDate = DateTime.Now.ToString( "yyyy/MM" );
        }

        public override void ArrangeViewBag ( int id = 0 ) {
            var workWithCompanyService = new ViewBusiness<AVBS_IngredientUnitPriceSelectListView>( );
            ViewBag.IngredientUnitPriceList = workWithCompanyService.GetAllQueryable().Select( x => new { x.Id, x.Name } ).ToList();
        }

        // TODO model bazlı kontrol yapılacak
        public override void EditSpecifications ( int id = 0 ) {

        }

        public override void CreateSpecifications ( int id = 0 ) {

        }

        public override void ModelOperations ( ref AVBS_ProductPurchase model ) {

        }

        [HttpPost]
        public JsonResult GetList (  JQueryDataTableParam param, string date ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var cashBookBusiness = new ViewBusiness<AVBS_ProductPurchaseTableView>();
                var allData = cashBookBusiness.GetAllQueryable(  x=>x.MonthYear == date );

                // Search
                if ( !string.IsNullOrEmpty( searchVal ) ) {
                    var searchQuery = searchVal.ToLower();
                    allData = allData.Where( x =>
                                            x.IngredientName.ToLower().Contains( searchQuery ) ||
                                            x.Detail.ToLower().Contains( searchQuery ) ||
                                            x.LabelName.ToLower().Contains( searchQuery ) ||
                                            x.UnitTypeName.ToLower().Contains( searchQuery ) ||
                                            x.WorkWithCompanyName.ToLower().Contains( searchQuery )
                    );
                }

                // Configure Order
                var orderingFunction = new Func<AVBS_ProductPurchaseTableView, object>( c => c.Id );
                if ( orderColumn == 0 ) {
                    orderingFunction = ( x => x.Date );
                } 

                // Order and arrange displayed data according to skip, take parameters
                var displayedData = orderDirection == "desc" ? allData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).ToList() : allData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).ToList();

                var totalNumber = allData.Count();
                decimal totalOutgoing = 0;

                if ( totalNumber > 0 ) totalOutgoing = allData.Sum( x => x.TotalPrice );


                return
                    Json(
                        new {
                            draw = param.draw,
                            recordsTotal = totalNumber,
                            recordsFiltered = totalNumber,
                            data = displayedData,
                            totalOutgoing = totalOutgoing
                        }, JsonRequestBehavior.AllowGet );
            } catch ( Exception e ) {

                return null;
            }
        }
         
    }
}