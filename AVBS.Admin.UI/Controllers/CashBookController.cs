using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Admin.UI.Controllers.Base;
using AVBS.Admin.UI.Models;
using AVBS.Business.Abstract;
using AVBS.Entity.Concrete;
using AVBS.Enum;
using AVBS.Infrastructure;
using AVBS.View.Concrete.Table;
using AVBS.Admin.UI.App_Data;
using AVBS.Business.Concrete;

namespace AVBS.Admin.UI.Controllers {
    [Authentication]
    public class CashBookController : IController< AVBS_CashBook, BaseBusiness<AVBS_CashBook>> {
         

        public override void ArrangeIndexViewBag () { 
             
        }

        public override void ArrangeViewBag ( int id = 0 ) {

        }

        // TODO model bazlı kontrol yapılacak
        public override void EditSpecifications ( int id = 0 ) {

        }

        public override void CreateSpecifications ( int id = 0 ) {

        }

        public override void ModelOperations ( ref AVBS_CashBook model ) {

        }

        public ActionResult Show( int id ) {
            var isDayClosed = _businessService.Get( id ).IsDayClosed;
            if ( isDayClosed )  return HttpNotFound();

            var cashBookBusiness = new ViewBusiness<AVBS_CashBookShowView>();
            var cashBook = cashBookBusiness.GetAllQueryable( x => x.Id == id ).FirstOrDefault();

            ViewBag.CashBook = cashBook;

            return View( );
        }

        public JsonResult GetList (  JQueryDataTableParam param, int locationid = 0 ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var cashBookBusiness = new ViewBusiness<AVBS_CashBookTableView>();
                var allData = cashBookBusiness.GetAllQueryable(   );
               

                // Configure Order
                var orderingFunction = new Func<AVBS_CashBookTableView, object>( c => c.Id );
                if ( orderColumn == 0 ) {
                    orderingFunction = ( x => x.Date );
                } else if ( orderColumn == 1 ) {
                    orderingFunction = ( x => x.CashStart );
                } else if ( orderColumn == 2 ) {
                    orderingFunction = ( x => x.CashEnd );
                } else if ( orderColumn == 3 ) {
                    orderingFunction = ( x => x.Bank );
                } else if ( orderColumn == 4 ) {
                    orderingFunction = ( x => x.TotalIncome );
                } else if ( orderColumn == 5 ) {
                    orderingFunction = ( x => x.TotalOutgoing );
                } else if ( orderColumn == 6 ) {
                    orderingFunction = ( x => x.IsDayClosed );
                } else if ( orderColumn == 7 ) {
                    orderingFunction = ( x => x.Turnover );
                }

                // Order and arrange displayed data according to skip, take parameters
                var displayedData = orderDirection == "desc" ? allData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).ToList() : allData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).ToList();

                var totalNumber = allData.Count();
                var totalOutgoing = allData.Sum( x => x.TotalOutgoing );

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

        public virtual JsonResult ChangeDayStatus ( int id ) {

            var cashbook = _businessService.Get( id );
            cashbook.IsDayClosed = !cashbook.IsDayClosed;

            var cashBookShowService = new ViewBusiness<AVBS_CashBookShowView>(  );
            var cashBookShow = cashBookShowService.GetAllQueryable( x => x.Id == id ).FirstOrDefault();
            if ( cashBookShow == null ) return null;

            cashbook.Turnover = cashBookShow.Turnover;
            cashbook.CashEnd = cashBookShow.CashEnd;
            cashbook.Bank = cashBookShow.Bank;
            var error = 0;
            if ( !_businessService.Update( cashbook ) ) error = 1;
            return Json( new { error } );
        }

        public ActionResult AddCashEndPartial ( int id = 0 ) {
            if ( id == 0 ) return HttpNotFound( );

            var cashBook = _businessService.Get( id );
            ViewBag.CashBookId = id;
            return PartialView( "AddCashEnd", cashBook );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddCashEnd ( AVBS_CashBook cashbookModel ) {

            var cashBook = _businessService.Get( cashbookModel.Id );
            cashBook.CashEnd = cashbookModel.CashEnd;
            cashBook.Bank = cashbookModel.Bank;

            if ( !_businessService.Update( cashBook ) ) {
                ViewBag.DbSuccess = false;
                return PartialView( "AddCashEnd" );
            }

            TempData["DbSuccess"] = true;
            return RedirectToAction( "Index" );
        }
    }
}