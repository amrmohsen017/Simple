using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PM.Models;

namespace PM.Controllers
{
    public class Task_logsController : Controller
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: Task_logs
        public ActionResult Index()
        {
            var task_logs = db.task_logs.Include(t => t.log).Include(t => t.task);
            return View(task_logs.ToList());
        }

        // GET: Task_logs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task_logs task_logs = db.task_logs.Find(id);
            if (task_logs == null)
            {
                return HttpNotFound();
            }
            return View(task_logs);
        }

        // GET: Task_logs/Create
        public ActionResult Create()
        {
            ViewBag.log_id = new SelectList(db.logs, "log_id", "log_text");
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "task_name");
            return View();
        }

        // POST: Task_logs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,task_id,log_id")] task_logs task_logs)
        {
            if (ModelState.IsValid)
            {
                db.task_logs.Add(task_logs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.log_id = new SelectList(db.logs, "log_id", "log_text", task_logs.log_id);
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "task_name", task_logs.task_id);
            return View(task_logs);
        }

        // GET: Task_logs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task_logs task_logs = db.task_logs.Find(id);
            if (task_logs == null)
            {
                return HttpNotFound();
            }
            ViewBag.log_id = new SelectList(db.logs, "log_id", "log_text", task_logs.log_id);
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "task_name", task_logs.task_id);
            return View(task_logs);
        }

        // POST: Task_logs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,task_id,log_id")] task_logs task_logs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task_logs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.log_id = new SelectList(db.logs, "log_id", "log_text", task_logs.log_id);
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "task_name", task_logs.task_id);
            return View(task_logs);
        }

        // GET: Task_logs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task_logs task_logs = db.task_logs.Find(id);
            if (task_logs == null)
            {
                return HttpNotFound();
            }
            return View(task_logs);
        }

        // POST: Task_logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            task_logs task_logs = db.task_logs.Find(id);
            db.task_logs.Remove(task_logs);
            db.SaveChanges();
            return RedirectToAction("Index");
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
