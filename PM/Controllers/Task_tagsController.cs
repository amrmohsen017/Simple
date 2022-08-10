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
    public class Task_tagsController : Controller
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: Task_tags
        public ActionResult Index()
        {
            var task_tags = db.task_tags.Include(t => t.tag).Include(t => t.task);
            return View(task_tags.ToList());
        }

        // GET: Task_tags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task_tags task_tags = db.task_tags.Find(id);
            if (task_tags == null)
            {
                return HttpNotFound();
            }
            return View(task_tags);
        }

        // GET: Task_tags/Create
        public ActionResult Create()
        {
            ViewBag.tag_id = new SelectList(db.tags, "tag_id", "tagname");
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "task_name");
            return View();
        }

        // POST: Task_tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,task_id,tag_id")] task_tags task_tags)
        {
            if (ModelState.IsValid)
            {
                db.task_tags.Add(task_tags);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tag_id = new SelectList(db.tags, "tag_id", "tagname", task_tags.tag_id);
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "task_name", task_tags.task_id);
            return View(task_tags);
        }

        // GET: Task_tags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task_tags task_tags = db.task_tags.Find(id);
            if (task_tags == null)
            {
                return HttpNotFound();
            }
            ViewBag.tag_id = new SelectList(db.tags, "tag_id", "tagname", task_tags.tag_id);
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "task_name", task_tags.task_id);
            return View(task_tags);
        }

        // POST: Task_tags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,task_id,tag_id")] task_tags task_tags)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task_tags).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tag_id = new SelectList(db.tags, "tag_id", "tagname", task_tags.tag_id);
            ViewBag.task_id = new SelectList(db.tasks, "task_id", "task_name", task_tags.task_id);
            return View(task_tags);
        }

        // GET: Task_tags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task_tags task_tags = db.task_tags.Find(id);
            if (task_tags == null)
            {
                return HttpNotFound();
            }
            return View(task_tags);
        }

        // POST: Task_tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            task_tags task_tags = db.task_tags.Find(id);
            db.task_tags.Remove(task_tags);
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
