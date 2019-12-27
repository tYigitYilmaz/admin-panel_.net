using System.Web.Mvc;
using AVBS.Office.UI.App_Data;

namespace AVBS.Office.UI.Controllers {
    [Authentication]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index() {

            return View();
        }

        
    }
}