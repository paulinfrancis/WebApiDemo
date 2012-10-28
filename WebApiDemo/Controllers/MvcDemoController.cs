using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApiDemo.Controllers
{
    public class MvcDemoController : Controller
    {
        //
        // GET: /Mvc/

        public ActionResult Index()
        {
            return View();
        }

    }
}
