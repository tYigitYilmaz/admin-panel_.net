using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Business.Abstract;
using AVBS.Business.Concrete;
using AVBS.Enum;
using AVBS.View.Concrete.Table;
using AVBS.Office.UI.App_Data;

namespace AVBS.Office.UI.Controllers {
    [Authentication]
    public class PageController : Controller
    { 

        public ActionResult CashBookMonthlyWidgets(  ) {

            //var date = DateTime.Now.ToString( "yyyy/MM" ); 
            
            //var cashBookBusiness = new ViewBusiness<AVBS_MonthlyOutgoingView>(  );
            //var cashbookMonthly = cashBookBusiness.GetAllQueryable( x => x.Date == date || x.OutgoingSourceTypeId == (int) OutgoingSourceType.Cash).FirstOrDefault();

            //ViewBag.CashBookMonthly = cashbookMonthly;

            return PartialView();
        }



        public ActionResult CashBookWidgets (int id) {

            //var cashBookBusiness = new ViewBusiness<AVBS_CashBookShowView>();
            //var cashBook = cashBookBusiness.GetAllQueryable( x => x.Id == id ).FirstOrDefault();
            //ViewBag.Cashbook = cashBook;

            return PartialView();
        }


        public ActionResult MonthlyAnalyticsWidgets ( string date ) {

            //if ( string.IsNullOrEmpty( date ) ) date = DateTime.Now.ToString( "yyyy/MM" );

            //var monthlyoutgoingservice = new ViewBusiness<AVBS_MonthlyOutgoingView>();
            //var monthlyOutgoing = monthlyoutgoingservice.GetAllQueryable( x => x.Date == date );

            //var cashbookMonthly = monthlyOutgoing.FirstOrDefault(x => x.OutgoingSourceTypeId == (int) OutgoingSourceType.Cash);
            //ViewBag.CashBookMonthly = cashbookMonthly;

            //var bankMonthlyOutgoing = monthlyOutgoing.FirstOrDefault( x => x.OutgoingSourceTypeId == (int) OutgoingSourceType.Bank );
            //ViewBag.MonthlyOutgoing = bankMonthlyOutgoing;


            //var cashbookShowViewService = new ViewBusiness<AVBS_CashBookMonthlyTurnoverView>();
            //var totalTurnover = cashbookShowViewService.GetAllQueryable( x => x.Date == date ).Select( x=>x.MonthlyTurnover ).FirstOrDefault();
            //ViewBag.TotalTurnover = totalTurnover;
            return PartialView();
        }

    }
}