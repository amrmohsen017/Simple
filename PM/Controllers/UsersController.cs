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
    public class UsersController : Controller
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: Users
        public ActionResult Index()
        {
            //var users = db.users.Include(u => u.institute).Include(u => u.job);
            return View();
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.institute_id = new SelectList(db.institutes, "institute_id", "institutename");
            ViewBag.job_id = new SelectList(db.jobs, "job_id", "jobname");
            return View();
        }

        // POST: Users/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,username,pass,telephone,email,job_id,institute_id,level_code")] user user)
        {
            if (ModelState.IsValid)
            {

                user.pass = Hash.Hash_this(user.pass);

                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.institute_id = new SelectList(db.institutes, "institute_id", "institutename", user.institute_id);
            ViewBag.job_id = new SelectList(db.jobs, "job_id", "jobname", user.job_id);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.institute_id = new SelectList(db.institutes, "institute_id", "institutename", user.institute_id);
            ViewBag.job_id = new SelectList(db.jobs, "job_id", "jobname", user.job_id);
            return View(user);
        }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,username,pass,telephone,email,job_id,institute_id,level_code")] user user)
        {
            if (ModelState.IsValid)
            {
                user.pass = Hash.Hash_this(user.pass);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.institute_id = new SelectList(db.institutes, "institute_id", "institutename", user.institute_id);
            ViewBag.job_id = new SelectList(db.jobs, "job_id", "jobname", user.job_id);
            return View(user);
        }

        // GET: Users/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    user user = db.users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    user user = db.users.Find(id);
        //    db.users.Remove(user);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
