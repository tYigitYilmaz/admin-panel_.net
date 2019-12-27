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
    public abstract class IOfficeReferenceController<TEntity> : Controller
        where TEntity : class, IOfficeReferenceEntity, new() {

        private BaseOfficeBusiness<TEntity> _businessService;

        protected IOfficeReferenceController ( )  {
            if ( TempData["DbSuccess"] != null ) ViewBag.DbSuccess = (bool) TempData["DbSuccess"];
            ViewBag.DbMessage = TempData["DbMessage"]?.ToString() ?? "İşlem esnasında bir hata oluşmuştur tekrar deneyiniz.";

        }

        protected override void Initialize ( System.Web.Routing.RequestContext requestContext ) {
            base.Initialize( requestContext );

            _businessService = new BaseOfficeBusiness<TEntity>( (AVBS_User) requestContext.HttpContext.Session["user"] );
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

            var returnText = entity.Name;
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

        public JsonResult GetList ( JQueryDataTableParam param, int locationid = 0 ) {

            Int32 orderColumn = Convert.ToInt32( Request["order[0][column]"] );
            String orderDirection = Convert.ToString( Request["order[0][dir]"] );
            String searchVal = Convert.ToString( Request["search[value]"] );

            try {
                var allData = _businessService.GetAllQueryable();

                // Search
                if ( !string.IsNullOrEmpty( searchVal ) ) {
                    var searchQuery = searchVal.ToLower();
                    allData = allData.Where( x =>
                                            x.Name.ToLower().Contains( searchQuery )
                    );
                }

                // Configure Order
                var orderingFunction = new Func<TEntity, object>( c => c.Id );
                if ( orderColumn == 0 ) {
                    orderingFunction = ( x => x.Name );
                }

                // Order and arrange displayed data according to skip, take parameters
                var displayedData = orderDirection == "asc" ? allData.OrderBy( orderingFunction ).Skip( param.start ).Take( param.length ).Select( x => new { x.Name, x.Id } ).ToList() : allData.OrderByDescending( orderingFunction ).Skip( param.start ).Take( param.length ).Select( x => new { x.Name, x.Id } ).ToList();

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



        // Virtual Classes
        public virtual void ArrangeViewBag ( int id = 0 ) { }
        public virtual void CreateSpecifications ( int id = 0 ) { }
        public virtual void EditSpecifications ( int id = 0) { }
        public virtual void ArrangeIndexViewBag ( ) { }
        public virtual void ModelOperations ( ref TEntity entity ) { }


    }

}