using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Office.UI.Controllers.Base;
using AVBS.Business.Abstract;
using AVBS.Business.Concrete;
using AVBS.Entity.Concrete;
using AVBS.Infrastructure; 
using AVBS.Office.UI.Helper; 

namespace AVBS.Office.UI.Controllers {
    public class LoginController : BaseController {
        // GET: Account
        public ActionResult Index () {

            if ( Session["user"] != null ) {
                return Redirect( "/home" );
            }
            return View();
        }

        public ActionResult Logout () {
            Session["user"] = null;
            Response.Cookies["userCookie"].Expires = DateTime.Now.AddDays( -1 );
            return Redirect( "/login" );
        }
        

        [HttpPost]
        public ActionResult Login ( string email, string password ) {
            if ( string.IsNullOrEmpty( email ) || string.IsNullOrEmpty( password ) ) {
                return Json( new { error = 1 } );
            }

            var officeBus = new BaseBusiness<AVBS_Office>(  );
            var officeId = 0;
            var office = officeBus.GetAllQueryable( x => x.Domain == Request.Url.Host ).FirstOrDefault();
            if ( office != null ) {
                officeId = office.Id;
            }

            var userBus = new BaseBusiness<AVBS_User>();
            var user = userBus.GetAllEnumerable( x => x.Email == email && x.OfficeId == officeId).FirstOrDefault();

            if ( user == null || user.Password.ToLower() != Md5Hash.CreateMD5( password).ToLower() ) {
                return Json( new { error = 2 } );
            }

            Session["user"] = user;
            CookieOperation.Insert( user, Response );

            return RedirectToAction( "Index", "Home" );

        }
    }
}