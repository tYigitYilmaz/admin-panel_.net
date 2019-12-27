using System;
using System.Web;
using AVBS.Entity.Concrete;
using AVBS.Infrastructure;
using AVBS.Office.UI.Models;
using Newtonsoft.Json;

namespace AVBS.Office.UI.Helper {

    // TODO convert it <T> class notation
	public static class CookieOperation
	{

		public static object Get(string key)
		{
			return HttpContext.Current.Cache.Get(key);
		}

		public static object Remove(string key)
		{
			return HttpContext.Current.Cache.Remove(key);
			;
		}

		public static void Insert(AVBS_User user, HttpResponseBase response)
		{
            CookieObject obj = new CookieObject();
            obj.ExpireDate = DateTime.Now.AddMonths( 6 );
            obj.UserId = user.Id;
            obj.Token = Guid.NewGuid().ToString().Substring( 0, 11 );
            obj.UserModel = UserHelper.ConvertToSessionModel(user);
		    HttpCookie cookie = new HttpCookie( "_userCookie" ) {
		        Value = StringCipher.Encrypt( JsonConvert.SerializeObject( obj ), "6304d4f8-18b7-42cd-9de9-6315256f9ec6" ),
		        Expires = DateTime.Now.AddMonths( 6 )
		    }; // TODO Change Name
		    response.Cookies.Add( cookie );


            // TODO Insert session table
            //SqlConnection conn = new SqlConnection( "Data Source=mssql35.natro.com;Initial Catalog=db9D8EE8;User ID=user9D8EE8;Password=VHqs32V8" );
            //conn.Open();
            //SqlCommand cmd = new SqlCommand( " Insert Into [LikitSatis.Session] values (@userId, @expireDate, @ip, @token, 1)", conn );
            //cmd.Parameters.AddWithValue( "@userId", obj.UserId );
            //cmd.Parameters.AddWithValue( "@expireDate", obj.ExpireDate );
            //cmd.Parameters.AddWithValue( "@ip", GetIP() );
            //cmd.Parameters.AddWithValue( "@token", obj.Token );
            //cmd.ExecuteNonQuery();
        }

		public static void Update(AVBS_User user, HttpResponseBase response)
		{



		}
	}
}
