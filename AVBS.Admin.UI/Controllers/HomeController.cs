using System.Web.Mvc;
using AVBS.Admin.UI.App_Data;

namespace AVBS.Admin.UI.Controllers {
    [Authentication]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index() {

            return View();
        }

        
    }
}