//using PM.Models;
using Microsoft.AspNet.SignalR;
using PM.Hubs;
using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;

namespace PM.Controllers
{
    public class HomeController : Controller
    {
        private project_managementEntities1 db = new project_managementEntities1();

        public ActionResult Index()
        {


            //var context = new project_managementEntities1();

            var hub = GlobalHost.ConnectionManager.GetHubContext<TestHub>();
           

            hub.Clients.All.controllerMethod("hi from mvc controller :)");


            //return Content(string.Join(",", context.institutes.Select(i => new { i.institute_address, i.institutename}).ToList()));
            ViewBag.task_supervisor = new SelectList(db.users, "user_id", "username");
            return View();
        }

        public ActionResult hub_context_from_action()
        {


            //var context = new project_managementEntities1();

            var hub = GlobalHost.ConnectionManager.GetHubContext<TestHub>();


            hub.Clients.All.controllerMethod("hi from mvc controller :)");

            

            //return Content(string.Join(",", context.institutes.Select(i => new { i.institute_address, i.institutename}).ToList()));
            return Content("damn :) " );
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