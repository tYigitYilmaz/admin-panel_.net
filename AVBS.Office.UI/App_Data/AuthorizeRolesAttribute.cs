﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AVBS.Entity.Concrete;
using AVBS.Enum;
namespace AVBS.Office.UI.App_Data {
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false )]
    public class AuthorizeRolesAttribute : AuthorizeAttribute {
        /// <summary>
        /// The name of the master page or view to use when rendering the view on authorization failure.  Default
        /// is null, indicating to use the master page of the specified view.
        /// </summary>
        public virtual string MasterName { get; set; }

        /// <summary>
        /// The name of the view to render on authorization failure.  Default is "Error".
        /// </summary>
        public virtual string ViewName { get; set; }


        private readonly int[] _roles;

        public AuthorizeRolesAttribute ( params Role[] roles ) : base() {
            _roles = roles.Cast<int>().ToArray();
            this.ViewName = "Error";
        }

        protected void CacheValidateHandler ( HttpContext context, object data, ref HttpValidationStatus validationStatus ) {
            validationStatus = OnCacheAuthorization( new HttpContextWrapper( context ) );
        }

        public override void OnAuthorization ( AuthorizationContext filterContext ) {

            var user = (AVBS_User) HttpContext.Current.Session["user"];
            if ( filterContext == null ) {
                throw new ArgumentNullException( "filterContext" );
            }

            if ( AuthorizeCore( filterContext.HttpContext ) ) {
                SetCachePolicy( filterContext );
            } else if ( user == null ) {
                filterContext.Result = new HttpUnauthorizedResult();
            } else if ( _roles.Contains( user.RoleId ) ) {
                SetCachePolicy( filterContext );
            } else {
                ViewDataDictionary viewData = new ViewDataDictionary();
                viewData.Add( "Message", "You do not have sufficient privileges for this operation." );
                filterContext.Result = new ViewResult { MasterName = this.MasterName, ViewName = this.ViewName, ViewData = viewData };
            }

        }

        protected void SetCachePolicy ( AuthorizationContext filterContext ) {
            // ** IMPORTANT **
            // Since we're performing authorization at the action level, the authorization code runs
            // after the output caching module. In the worst case this could allow an authorized user
            // to cause the page to be cached, then an unauthorized user would later be served the
            // cached page. We work around this by telling proxies not to cache the sensitive page,
            // then we hook our custom authorization code into the caching mechanism so that we have
            // the final say on whether a page should be served from the cache.
            HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
            cachePolicy.SetProxyMaxAge( new TimeSpan( 0 ) );
            cachePolicy.AddValidationCallback( CacheValidateHandler, null /* data */);
        }

    }
}