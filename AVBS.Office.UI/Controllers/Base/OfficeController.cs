using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AVBS.Office.UI.Models;
using AVBS.Business.Abstract;
using AVBS.Business.Concrete;
using AVBS.Entity.Abstract;
using AVBS.Entity.Concrete;
using AVBS.Infrastructure;
using AVBS.Office.UI.App_Data;

namespace AVBS.Office.UI.Controllers.Base {

    [UserCache]
    [Authentication]
    public abstract class OfficeController<TEntity> : Controller
        where TEntity : class, IOfficeEntity, new() {

        protected BaseOfficeBusiness<TEntity> _businessService;
        protected AVBS_User _loggedUser;

        protected OfficeController ( )  {
            if ( TempData["DbSuccess"] != null ) ViewBag.DbSuccess = (bool) TempData["DbSuccess"];
            ViewBag.DbMessage = TempData["DbMessage"]?.ToString() ?? "İşlem esnasında bir hata oluşmuştur tekrar deneyiniz.";

        }

        protected override void Initialize ( System.Web.Routing.RequestContext requestContext ) {
            base.Initialize( requestContext );

            _loggedUser = ( AVBS_User ) requestContext.HttpContext.Session[ "user" ];
            _businessService = new BaseOfficeBusiness<TEntity>( _loggedUser );
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
            return PartialView( );
        }

        public ActionResult CreateWithReturn ( int id = 0 ) {
            ArrangeViewBag( id );
            CreateSpecifications( id );
            return PartialView( "CreateWithReturn" );
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

            return PartialView( entity );
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


        // Virtual Classes
        public virtual void ArrangeViewBag ( int id = 0 ) { }
        public virtual void CreateSpecifications ( int id = 0 ) { }
        public virtual void EditSpecifications ( int id = 0) { }
        public virtual void ArrangeIndexViewBag ( ) { }
        public virtual void ModelOperations ( ref TEntity entity ) { }


    }

}