using System.Web.Mvc;

namespace AVBS.Office.UI.Areas.References
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
                "Office_References",
                "References/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}