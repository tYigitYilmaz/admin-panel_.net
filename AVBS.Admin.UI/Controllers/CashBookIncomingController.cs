using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Admin.UI.Controllers.Base;
using AVBS.Admin.UI.Models;
using AVBS.Business.Abstract;
using AVBS.Entity.Concrete;
using AVBS.Entity.References;
using AVBS.Enum;
using AVBS.Infrastructure;
using AVBS.View.Concrete.Table;
using AVBS.Admin.UI.App_Data;
using AVBS.Business.Concrete;

namespace AVBS.Admin.UI.Controllers {
    [Authentication]
    public class CashBookIncomingController : IController< AVBS_CashBookIncoming, BaseBusiness<AVBS_CashBookIncoming>> {

        public PartialViewResult IndexPartial( int cashbookid ) {
            ViewBag.CashBookId = cashbookid;
            return PartialView("Index");
        }

        public override void ArrangeIndexViewBag () { 
             
        }

        public override void ArrangeViewBag ( int id = 0 ) {
            var incomingTypeService = new BaseBusiness<AVBS_IncomingType>();
            ViewBag.IncomingTypeList = incomingTypeService.GetAllQueryable().Select( x => new { x.Id, x.Name } ).ToList();

            var creditCardTypeService = new BaseBusiness<AVBS_CreditCardType>();
            ViewBag.CreditCardTypeList = creditCardTypeService.GetAllQueryable().Select( x => new { x.Id, x.Name } ).ToList();
        }

        // TODO model bazlı kontrol yapılacak
        public override void EditSpecifications ( int id = 0 ) {
            var cashbookincoming = _businessService.Get( id );

            ViewBag.CashBook = cashbookincoming.AVBS_CashBook;
        }


        public override void CreateSpecifications ( int id = 0 ) {
            var cashbookService = new BaseBusiness<AVBS_CashBook>();
            var cashbook = cashbookService.Get( id );

            ViewBag.CashBook = cashbook;
        }

        public override void ModelOperations ( ref AVBS_CashBookIncoming model ) {

        }
 

        public JsonResult GetList ( int cashbookid, JQueryDataTableParam param ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var cashBookBusiness = new ViewBusiness<AVBS_CashBookIncomingTableView>();
                var allData = cashBookBusiness.GetAllQueryable( x=> x.CashBookId == cashbookid );

                // Search
                if ( !string.IsNullOrEmpty( searchVal ) ) {
                    var searchQuery = searchVal.ToLower();
                    allData = allData.Where( x => 
                                            x.IncomingTypeName.ToLower().Contains(searchQuery) ||
                                            x.CreditCardTypeName.ToLower().Contains( searchQuery )
                    );
                }

                // Configure Order
                var orderingFunction = new Func<AVBS_CashBookIncomingTableView, object>( c => c.Id );
                if ( orderColumn == 0 ) {
                    orderingFunction = ( x => x.Date );
                } else if ( orderColumn == 1 ) {
                    orderingFunction = ( x => x.IncomingTypeId );
                } else if ( orderColumn == 2 ) {
                    orderingFunction = ( x => x.CreditCardTypeId );
                } else if ( orderColumn == 3 ) {
                    orderingFunction = ( x => x.CommissionRate );
                } else if ( orderColumn == 4 ) {
                    orderingFunction = ( x => x.Amount );
                }

                // Order and arrange displayed data according to skip, take parameters
                var displayedData = orderDirection == "asc" ? allData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).ToList() : allData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).ToList();

                var totalNumber = allData.Count();
                decimal totalIncome = 0;

                if ( totalNumber > 0 ) totalIncome = allData.Sum( x => x.Amount );

                return
                    Json(
                        new {
                            draw = param.draw,
                            recordsTotal = totalNumber,
                            recordsFiltered = totalNumber,
                            data = displayedData,
                            totalIncome = totalIncome
                        }, JsonRequestBehavior.AllowGet );
            } catch ( Exception e ) {

                return null;
            }

        }
 


    }
}