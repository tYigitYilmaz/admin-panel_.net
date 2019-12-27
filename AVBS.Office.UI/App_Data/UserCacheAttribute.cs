using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AVBS.Business.Abstract; 
using AVBS.Infrastructure; 

namespace AVBS.Office.UI.App_Data
{
    public class UserCacheAttribute : ActionFilterAttribute {
        public override void OnActionExecuting ( ActionExecutingContext filterContext ) {
            var cache = CacheOperation.Get("UserCache");
            //if ( cache == null ) {
            //    var userCacheViewService = new ViewBusiness<Tracker_UserCacheView>(  );

            //    UserCacheModel userCacheModel = new UserCacheModel(  ) {
            //        UserCacheList = userCacheViewService.GetAllQueryable( x=> x.CompanyId == 3 ).ToList()
            //    };

            //    CacheOperation.Insert( "UserCache", userCacheModel );
            //}

            base.OnActionExecuting( filterContext );
           
        }
    }
}