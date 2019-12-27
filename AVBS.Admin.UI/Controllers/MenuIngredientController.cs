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
    public class MenuIngredientController : IController<AVBS_MenuIngredient, BaseBusiness<AVBS_MenuIngredient>> {

        public override ActionResult Index ( int id = 0 ) {
            var menuService = new BaseBusiness<AVBS_Menu>();
            var menu = menuService.Get( id );

            ViewBag.Menu = menu;
            return View( "Index" );
        }

        public override void ArrangeIndexViewBag () {

        }

        public override void ArrangeViewBag ( int id = 0 ) {

            var ingredientService = new BaseBusiness<AVBS_Ingredient>();
            var ingredientList = ingredientService.GetAllQueryable( ).Select( x => new {x.Id, x.Name} );
              
            // TODO seçeneğe göre ajax ile dinamik eklenecek ( örneğin kaşar seçilmişse kilo bazlı, süt seçilmişse ml bazlı)
            var unitTypeService = new BaseBusiness<AVBS_UnitType>();
            var unitTypeList = unitTypeService.GetAllQueryable( ).Select( x => new {x.Id, x.Name} );

            ViewBag.IngredientList = ingredientList;
            ViewBag.UnitTypeList = unitTypeList; 

        }

        // TODO model bazlı kontrol yapılacak
        public override void EditSpecifications ( int id = 0 ) {
            var menuingredient = _businessService.Get( id );

            ViewBag.Menu = menuingredient.AVBS_Menu;

            TempData["ReturnId"] = menuingredient.MenuId;
        }

        public override void CreateSpecifications ( int id = 0 ) {
            var menuService = new BaseBusiness<AVBS_Menu>();
            var menu = menuService.Get( id );

            TempData["ReturnId"] = id;
            ViewBag.Menu = menu;
        }

        public override void ModelOperations ( ref AVBS_MenuIngredient model ) {

        }

        public JsonResult GetList ( JQueryDataTableParam param, int menuid = 0 ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var cashBookBusiness = new ViewBusiness<AVBS_MenuIngredientTableView>();
                var allData = cashBookBusiness.GetAllQueryable( x=>x.MenuId == menuid );

                // Search
                if ( !string.IsNullOrEmpty( searchVal ) ) {
                    var searchQuery = searchVal.ToLower();
                    allData = allData.Where( x =>
                                            x.MenuName.ToLower().Contains( searchQuery ) ||
                                            x.IngredientName.ToLower().Contains( searchQuery ) ||
                                            x.UnitTypeName.ToLower().Contains( searchQuery ) 
                    );
                }

                // Configure Order
                var orderingFunction = new Func<AVBS_MenuIngredientTableView, object>( c => c.Id );
                if ( orderColumn == 0 ) {
                    //orderingFunction = ( x => x.Name );
                }

                // Order and arrange displayed data according to skip, take parameters
                var displayedData = orderDirection == "asc" ? allData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).ToList() : allData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).ToList();

                var totalNumber = allData.Count();
                var totalPrice = allData.Sum( x => (double?) ( x.Price ) ) ?? 0; ;
                return
                    Json(
                        new {
                            draw = param.draw,
                            recordsTotal = totalNumber,
                            recordsFiltered = totalNumber,
                            data = displayedData,
                            totalPrice = totalPrice
                        }, JsonRequestBehavior.AllowGet );
            } catch ( Exception e ) {

                return null;
            }
        }

    }
}