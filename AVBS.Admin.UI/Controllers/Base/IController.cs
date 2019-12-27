using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AVBS.Admin.UI.Models;
using AVBS.Business.Abstract;  
using AVBS.Entity.Abstract;
using AVBS.Enum;
using AVBS.Admin.UI.App_Data;
using AVBS.Admin.UI.Models;

namespace AVBS.Admin.UI.Controllers.Base {

    [UserCache]
    [Authentication]
    public abstract class IController<TEntity, TBusiness> : Controller
        where TEntity : class, IEntity, new()
        where TBusiness : class, IBusiness<TEntity>, new() {

        public readonly TBusiness _businessService = new TBusiness();
        protected DateTime _startDate; DateTime _endDate;
        protected int _dayCount = 0;

        protected IController () {
           
            if ( TempData["DbSuccess"] != null ) ViewBag.DbSuccess = (bool) TempData["DbSuccess"];
             ViewBag.DbMessage = TempData["DbMessage"]?.ToString(  ) ?? "İşlem esnasında bir hata oluşmuştur tekrar deneyiniz.";
        }

        public virtual ActionResult Index (int id = 0) {
            if ( TempData["DbSuccess"] != null ) ViewBag.DbSuccess = (bool) TempData["DbSuccess"];
            ArrangeIndexViewBag();

            return View(  );
        }

        public ActionResult Create (int id = 0) {
            ArrangeViewBag( id );
            CreateSpecifications( id );
            return View();
        }

        public ActionResult CreatePartial (int id = 0) {
            ArrangeViewBag(id);
            CreateSpecifications( id );
            return PartialView("Create" );
        }

        public ActionResult CreateWithReturn ( int id = 0 ) {
            ArrangeViewBag( id );
            CreateSpecifications( id );
            return PartialView( "CreateWithReturn" );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create ( TEntity entity ) {
            if ( !ModelState.IsValid ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag();
                return View( entity );
            }

            ModelOperations( ref entity );
            if ( !_businessService.Add( entity ) ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag();
                return View( entity );
            }

            TempData["DbSuccess"] = true;

            if ( TempData["ReturnId"] != null ) return RedirectToAction("Index", new { id = TempData["ReturnId"]} );
            return RedirectToAction( "Index" );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual JsonResult CreateWithReturn ( TEntity entity ) {
            if ( !ModelState.IsValid ) {
                return Json( new { error = 1, message = "Error" } );
            }

            ModelOperations( ref entity );

            entity = _businessService.AddWithReturnEntity( entity );

            if ( entity.Id == 0 ) {
                return Json( new { error = 1, message = "Error" } );
            }

            var returnText = entity.GetType().GetProperty( "Name" ).GetValue( entity, null );
            return Json( new { error = 0, value = new { id = entity.Id, text = returnText } } );
        }

        public virtual ActionResult Edit ( int id ) {
            ArrangeViewBag(id);
            EditSpecifications(id);
            TEntity entity = _businessService.GetAsNoTracking( id );

            return View( entity );
        }

        public virtual ActionResult EditPartial ( int id ) {
            ArrangeViewBag( id );
            EditSpecifications( id );
            TEntity entity = _businessService.GetAsNoTracking( id );

            return PartialView( "Edit", entity );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit ( TEntity entity ) {
            if ( !ModelState.IsValid ) {
                ViewBag.DbSuccess = false;
                ArrangeViewBag( entity.Id);
                EditSpecifications( entity.Id );
                return View( entity );
            }

            ModelOperations( ref entity );
            if ( !_businessService.Update( entity ) ) {
                ViewBag.DbSuccess = false;
                entity = _businessService.Get( entity.Id );
                ArrangeViewBag( entity.Id);
                EditSpecifications( entity.Id );
                return View( entity );
            }

            TempData["DbSuccess"] = true;
            if ( TempData["ReturnId"] != null ) return RedirectToAction( "Index", new { id = TempData["ReturnId"] } );
            return RedirectToAction( "Index" );
        }

        public virtual JsonResult Delete ( int id ) {
            var error = 0;
            if ( ! _businessService.Remove( id ) ) error = 1 ;
            return Json( new { error } );
        }

        protected string GetIP () {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if ( !string.IsNullOrEmpty( ipAddress ) ) {
                string[] addresses = ipAddress.Split( ',' );
                if ( addresses.Length != 0 ) {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public ActionResult NotFound () {
            Server.ClearError();
            Response.Status = "404 Not Found";
            Response.StatusCode = 404;
            return View();
        }

        public List<NotificationModel> GetNotifications ( int id ) {
            List<NotificationModel> model = new List<NotificationModel>();
            return model;

        }


        protected int GetDayCount ( int periodType ) {
            var dayCount = 0;
            switch ( periodType ) {
                case (int) PeriodType.Today:
                    dayCount = 0;
                    break;
                case (int) PeriodType.Week:
                    dayCount = 7;
                    break;
                case (int) PeriodType.Month:
                    dayCount = 30;
                    break;
                case (int) PeriodType.Specific:
                    dayCount = 0;
                    break;
                default:
                    return -1;
            }

            return dayCount;

        }

        protected void CalculateFromToDates ( CashBookPageParam cashBookPageParam ) {
            var fromDate = cashBookPageParam.FromDate;
            var toDate = cashBookPageParam.ToDate;
            if ( fromDate != null ) {
                if ( toDate != null ) {
                    _dayCount = ( toDate - fromDate ).Value.Days + 1;
                    _startDate = fromDate.Value;
                    _endDate = toDate.Value;
                } else {

                    _startDate = fromDate.Value;
                    _endDate = fromDate.Value.AddDays( 1 ).AddTicks( -1 );
                }

            } else {
                _startDate = DateTime.Now.Date.AddDays( -_dayCount + 1 );
                _endDate = DateTime.Now.Date.AddDays( 1 ).AddTicks( -1 );
            }
        }



        // Virtual Classes
        public virtual void ArrangeViewBag ( int id = 0 ) { }
        public virtual void CreateSpecifications ( int id = 0 ) { }
        public virtual void EditSpecifications ( int id = 0) { }
        public virtual void ArrangeIndexViewBag ( ) { }
        public virtual void ModelOperations ( ref TEntity entity ) { }


    }

}