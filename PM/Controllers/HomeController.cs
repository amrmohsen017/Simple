//using PM.Models;
using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {


            var context = new project_managementEntities1();

            //return Content(string.Join(",", context.institutes.Select(i => new { i.institute_address, i.institutename}).ToList()));
            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}