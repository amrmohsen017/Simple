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
    public class AttatchmentsController : Controller
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: Attachemnts
        public ActionResult Index()
        {
            return View(db.attachemnts.ToList());
        }

        // GET: Attachemnts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            attachemnt attachemnt = db.attachemnts.Find(id);
            if (attachemnt == null)
            {
                return HttpNotFound();
            }
            return View(attachemnt);
        }

        // GET: Attachemnts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Attachemnts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "attachment_id,attachment_name")] attachemnt attachemnt)
        {
            if (ModelState.IsValid)
            {
                db.attachemnts.Add(attachemnt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(attachemnt);
        }

        // GET: Attachemnts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            attachemnt attachemnt = db.attachemnts.Find(id);
            if (attachemnt == null)
            {
                return HttpNotFound();
            }
            return View(attachemnt);
        }

        // POST: Attachemnts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "attachment_id,attachment_name")] attachemnt attachemnt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attachemnt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attachemnt);
        }

        // GET: Attachemnts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            attachemnt attachemnt = db.attachemnts.Find(id);
            if (attachemnt == null)
            {
                return HttpNotFound();
            }
            return View(attachemnt);
        }

        // POST: Attachemnts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            attachemnt attachemnt = db.attachemnts.Find(id);
            db.attachemnts.Remove(attachemnt);
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
