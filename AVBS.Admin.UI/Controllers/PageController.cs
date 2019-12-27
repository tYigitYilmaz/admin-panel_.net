using System;
using System.Linq;
using System.Web.Mvc;
using AVBS.Business.Abstract;
using AVBS.Enum;
using AVBS.View.Concrete.Table;
using AVBS.Admin.UI.App_Data;

namespace AVBS.Admin.UI.Controllers {
    [Authentication]
    public class PageController : Controller
    { 

        public ActionResult Index(  ) {
            
            return View();
        }



    }
}