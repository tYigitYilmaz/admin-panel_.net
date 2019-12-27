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
    public class IngredientUnitPriceController : IController<AVBS_IngredientUnitPrice, BaseBusiness<AVBS_IngredientUnitPrice>> {


        public override void ArrangeIndexViewBag () {

        }

        public override void ArrangeViewBag ( int id = 0 ) {

            var ingredientLabelCompanyService = new ViewBusiness<AVBS_IngredientLabelCompanySelectListView>();
            ViewBag.IngredientLabelCompanyList = ingredientLabelCompanyService.GetAllQueryable().Select( x => new { x.Id, x.Name } ).ToList();

            var unitTypeService = new BaseBusiness<AVBS_UnitType>();
            ViewBag.UnitTypeList = unitTypeService.GetAllQueryable().Select( x => new { x.Id, x.Name } ).ToList();
        }

        // TODO model bazlı kontrol yapılacak
        public override void EditSpecifications ( int id = 0 ) {

        }

        public override void CreateSpecifications ( int id = 0 ) {

        }

        public override void ModelOperations ( ref AVBS_IngredientUnitPrice model ) {

        }

        public JsonResult GetList ( JQueryDataTableParam param ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var cashBookBusiness = new ViewBusiness<AVBS_IngredientUnitPriceTableView>();
                var allData = cashBookBusiness.GetAllQueryable();

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
                var orderingFunction = new Func<AVBS_IngredientUnitPriceTableView, object>( c => c.Id );
                if ( orderColumn == 0 ) {
                    orderingFunction = ( x => x.Date );
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


        public JsonResult GetValues (int id) {

            try {

                var values = _businessService.Get( id );

                return Json( new {unitprice = values.UnitPrice, taxcutpercentage = values.TaxCutPercentage},
                    JsonRequestBehavior.AllowGet );
            } catch ( Exception e ) {
                return
                   Json(
                       new {
                          error = 0
                       }, JsonRequestBehavior.AllowGet );
            }

        }
    }
}