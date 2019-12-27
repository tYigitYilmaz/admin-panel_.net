using System.Collections.Generic; 
using System.Web.Mvc; 
using AVBS.Admin.UI.Models;

namespace AVBS.Admin.UI.Controllers.Base {
    public class BaseController : Controller {

        public BaseController () {
            if ( TempData["DbSuccess"] != null ) ViewBag.DbSuccess = (bool) TempData["DbSuccess"];
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

        public List<NotificationModel> GetNotifications ( int shopId ) {
            List<NotificationModel> model = new List<NotificationModel>();
            return model;

        }

    }

}