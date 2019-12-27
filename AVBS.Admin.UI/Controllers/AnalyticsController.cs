using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Admin.UI.App_Data;
using AVBS.Admin.UI.Models.Analytic;
using AVBS.Business.Abstract;
using AVBS.Entity.Concrete;
using AVBS.View.Concrete.SelectList;
using AVBS.View.Concrete.Table;
using AVBS.Admin.UI.App_Data;
using AVBS.Business.Concrete;

namespace AVBS.Admin.UI.Controllers {
    [Authentication]
    public class AnalyticsController : Controller {

        public ActionResult Index ( string date ) {

            var monthTermService = new BaseBusiness<AVBS_MonthlyTerm>( );
            var monthTermList = monthTermService.GetAllQueryable().Select( x => new { Id = x.Name, Name = x.Name } ).ToList();

            if ( string.IsNullOrEmpty( date ) ) date = DateTime.Now.ToString( "yyyy/MM" );

            ViewBag.MonthTermList = monthTermList;
            ViewBag.CurrentDate = date; 
            return View();
        }


        public JsonResult GetOutgoingPageData ( string date ) {

            OutgoingAnalyticModel outgoingAnalyticModel = new OutgoingAnalyticModel();

            try {
                
                var monthlyAmountByOutgoingTypeService = new ViewBusiness<AVBS_MonthlyAmountByOutgoingTypeView>();
                var monthlyAmountByOutgoingType = monthlyAmountByOutgoingTypeService.GetAllQueryable( x => ( x.Date == date )).ToList();

                outgoingAnalyticModel.AmountByOutgoingTypeModel = new AmountByOutgoingTypeModel {
                    Amount = monthlyAmountByOutgoingType.Select( x => x.MonthlyOutgoing ).ToArray(),
                    XAxis = monthlyAmountByOutgoingType.Select( x => x.OutgoingTypeName ).ToArray()
                };

                var monthlyAmountBySubOutgoingTypeService = new ViewBusiness<AVBS_MonthlyAmountBySubOutgoingTypeView>();
                var monthlyAmountBySubOutgoingType = monthlyAmountBySubOutgoingTypeService.GetAllQueryable( x => ( x.Date == date ) ).ToList();

                outgoingAnalyticModel.AmountBySubOutgoingTypeModel = new AmountBySubOutgoingTypeModel {
                    Amount = monthlyAmountBySubOutgoingType.Select( x => x.MonthlyOutgoing ).ToArray(),
                    XAxis = monthlyAmountBySubOutgoingType.Select( x => x.SubOutgoingTypeName ).ToArray()
                };

                return Json( outgoingAnalyticModel, JsonRequestBehavior.AllowGet );
            } catch ( Exception e ) {
                return null;
            }

        }


    }
}