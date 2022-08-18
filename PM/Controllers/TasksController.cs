using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;

using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using PM.Hubs;
using PM.Models;
using PM.ViewModels;

namespace PM.Controllers
{
    public class TasksController : Controller
    {
        private project_managementEntities1 db = new project_managementEntities1();


        public ActionResult Dashboard()
        {
            ViewBag.task_supervisor = new SelectList(db.users, "user_id", "username");
            return View();
        }


        // GET: Tasks
        public ActionResult Index()
        {
            var tasks = db.tasks.Include(t => t.user);
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            var statuses = db.status.ToList();
            var attatchments = db.attachemnts.ToList();
            var tags = db.tags.ToList();

            var task_stuff = new TaskInfo { task = task ,  state = statuses, tags = tags , attatchments = attatchments };

            //ViewBag.tag_id = new SelectList(db.tags, "tag_id", "tag_name");
            //ViewBag.status_id = new SelectList(db.status, "departementid", "departmentname");
            //ViewBag.attachemnt_id = new SelectList(db.attachemnts, "departementid", "departmentname");

            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task_stuff);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.task_supervisor = new SelectList(db.users, "user_id", "username");
            return View();
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "task_id,task_name,task_planned_start,task_planned_end,task_deadline,sub_task,task_supervisor,task_description,status_id")] task task)
        {
            if (ModelState.IsValid)
            {
                
                db.tasks.Add(task);
                db.SaveChanges();
                var hub = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();


                hub.Clients.All.controllerMethod(task.task_name , task.task_description);



                //return RedirectToAction("Index" , "Home");
            }

            ViewBag.task_supervisor = new SelectList(db.users, "user_id", "username", task.task_supervisor);
            ViewBag.status_id = new SelectList(db.status, "status_id", "status_name", task.task_status);
            return View(task);
            //return View("Home" , ,task);


        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.task_supervisor = new SelectList(db.users, "user_id", "username", task.task_supervisor);
            ViewBag.status_id = new SelectList(db.status, "status_id", "status_name", task.task_status);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "task_id,task_name,task_planned_start,task_planned_end,task_deadline,sub_task,task_supervisor,task_description,status_id")] task task)
        {
            

            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            ViewBag.task_supervisor = new SelectList(db.users, "user_id", "username", task.task_supervisor);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            task task = db.tasks.Find(id);
            //

            // task has : tags , logs , attachments ,assigned employees . to be deleted

            db.task_logs.RemoveRange(db.task_logs.Where(l => l.task_id == id));


            db.task_tags.RemoveRange(db.task_tags.Where(l => l.task_id == id));


            db.task_assignedemployee.RemoveRange(db.task_assignedemployee.Where(l => l.task_id == id));

            //this call doesn't trigger the physical deletion from disk 
            db.task_attachments.RemoveRange(db.task_attachments.Where(l => l.task_id == id));

            db.SaveChanges();



            //if (sub_task_id != 0)
            //{
            //    var parent_task = db.tasks.Find(taskId);
            //    parent_task.sub_task = null;

            //}
            //else// case of deleting a sub_task as a task
            //{


            //}


            //
            db.tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {

            string task_id = (string)ControllerContext.RouteData.Values["id"];
            //var task_id = Request.QueryString["id"];
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Attachments"), _FileName);
                    file.SaveAs(_path);
                    string url = Url.Content(Path.Combine("~/Attachments/", _FileName));

                    var new_attachment = new attachemnt { attachment_name = _FileName, attachment_path = _path, attachment_url = url };

                    db.attachemnts.Add(new_attachment);
                    db.SaveChanges();
                    db.task_attachments.Add(new task_attachments { task_id = int.Parse(task_id), attachmnet_id = new_attachment.attachment_id });
                    db.SaveChanges();

                    //db.task_attachments.Include(t => t.attachemnt.attachment_name).Where(t => t.task_id == int.Parse(task_id)).Select(t => new { t.task_id, t.attachmnet_id, t.attachemnt.attachment_name });




                }
                ViewBag.Message = "Attachment Uploaded Successfully!!";
                return RedirectToAction("Edit" , new { id = int.Parse(task_id) });
                
            }
            catch
            {
                ViewBag.Message = "Attachment upload failed!!";
                return  RedirectToAction("Edit", new { id = int.Parse(task_id) });
            }
        }


     


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
