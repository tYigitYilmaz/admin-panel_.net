using Newtonsoft.Json;
using System.Web;
using System.Web.Mvc;
using AVBS.Infrastructure;
using AVBS.Office.UI.Models;

namespace AVBS.Office.UI.App_Data {
    public class AuthenticationAttribute : ActionFilterAttribute {
        public override void OnActionExecuting ( ActionExecutingContext filterContext ) {
            if ( HttpContext.Current.Session["user"] != null ) {
                base.OnActionExecuting( filterContext );
                return;
            }
            HttpCookie aCookie = HttpContext.Current.Request.Cookies["userCookie"];
            if ( aCookie != null ) // Cookie
            {
                var decryptedValue = StringCipher.Decrypt( aCookie.Value, "6304d4f8-18b7-42cd-9de9-6315256f9ec6" );
                var userCookie = JsonConvert.DeserializeObject<CookieObject>( decryptedValue );

                if ( CheckCookie( userCookie ) ) {
                    HttpContext.Current.Session["user"] = userCookie.UserModel;
                }
            } else {
                filterContext.Result = new RedirectResult( "/login" );
            }

        }

        private bool CheckCookie ( CookieObject obj ) {
            // TODO Db kontrol
            //SqlConnection conn = new SqlConnection( "Data Source=mssql35.natro.com;Initial Catalog=db9D8EE8;User ID=user9D8EE8;Password=VHqs32V8" );
            //conn.Open();
            //SqlCommand cmd = new SqlCommand( " Select * from [LikitSatis.Session] where Token = @token and UserId= @userId and IsShop = 1", conn );
            //cmd.Parameters.AddWithValue( "@token", obj.Token );
            //cmd.Parameters.AddWithValue( "@userId", obj.UserId );

            //var dr = cmd.ExecuteReader();
            //var returnObj = dr.HasRows;
            //conn.Close();
            //dr.Close();
            return true;

        }


    }
}