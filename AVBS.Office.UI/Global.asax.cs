using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AVBS.Infrastructure;

namespace AVBS.Office.UI {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start () {

            ViewEngines.Engines.Clear(); //clear all engines
            ViewEngines.Engines.Add( new RazorViewEngine() );

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            BundleConfig.RegisterBundles( BundleTable.Bundles );

            ModelBinders.Binders.Add( typeof( DateTime ), new DateTimeBinder() );
            ModelBinders.Binders.Add( typeof( DateTime? ), new NullableDateTimeBinder() );
        }

        protected void Application_BeginRequest ( Object sender, EventArgs e ) {
            CultureInfo newCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.LongTimePattern = "dd/MM/yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }

        //protected void Application_Error ( object sender, EventArgs e ) {

        //    Response.Redirect( "/Error/Index/" );
        //}
    }
}
