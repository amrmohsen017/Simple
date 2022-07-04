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
    public class Institute_typeController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: institute_type
        public ActionResult Index()
        {
            return View(db.institute_type.ToList());
        }

        // GET: institute_type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            institute_type institute_type = db.institute_type.Find(id);
            if (institute_type == null)
            {
                return HttpNotFound();
            }
            return View(institute_type);
        }

        // GET: institute_type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: institute_type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "type_id,typename")] institute_type institute_type)
        {
            if (ModelState.IsValid)
            {
                db.institute_type.Add(institute_type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(institute_type);
        }

        // GET: institute_type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            institute_type institute_type = db.institute_type.Find(id);
            if (institute_type == null)
            {
                return HttpNotFound();
            }
            return View(institute_type);
        }

        // POST: institute_type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "type_id,typename")] institute_type institute_type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(institute_type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(institute_type);
        }

        // GET: institute_type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            institute_type institute_type = db.institute_type.Find(id);
            if (institute_type == null)
            {
                return HttpNotFound();
            }
            return View(institute_type);
        }

        // POST: institute_type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            institute_type institute_type = db.institute_type.Find(id);
            db.institute_type.Remove(institute_type);
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
