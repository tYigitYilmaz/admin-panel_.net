using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Admin.UI.Controllers.Base;
using AVBS.Business.Abstract;
using AVBS.Entity.Concrete;
using AVBS.Infrastructure; 
using AVBS.Admin.UI.Helper;
using AVBS.Business.Concrete;

namespace AVBS.Admin.UI.Controllers {
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

            var userBus = new BaseBusiness<AVBS_User>();
            var user = userBus.GetAllEnumerable( x => x.Email == email).FirstOrDefault();

            if ( user == null || user.Password.ToLower() != Md5Hash.CreateMD5( password).ToLower() ) {
                return Json( new { error = 2 } );
            }

            Session["user"] = user;
            CookieOperation.Insert( user, Response );

            return RedirectToAction( "Index", "Home" );

        }
    }
}