using System.Web.Mvc;

namespace AVBS.Admin.UI.Areas.References
{
    public class ReferencesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "References";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "References_default",
                "References/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}