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
    public class institute_addressController : Controller
    {
        private project_managementEntities db = new project_managementEntities();

        // GET: institute_address
        public ActionResult Index()
        {
            var institute_address = db.institute_address.Include(i => i.city).Include(i => i.governmnet).Include(i => i.station);
            return View(institute_address.ToList());
        }

        // GET: institute_address/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            institute_address institute_address = db.institute_address.Find(id);
            if (institute_address == null)
            {
                return HttpNotFound();
            }
            return View(institute_address);
        }

        // GET: institute_address/Create
        public ActionResult Create()
        {
            ViewBag.citycode = new SelectList(db.cities, "citycode", "cityname");
            ViewBag.governmnetcode = new SelectList(db.governmnets, "governmentcode", "governmentname");
            ViewBag.stationcode = new SelectList(db.stations, "stationcode", "stationname");
            return View();
        }

        // POST: institute_address/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "institute_adress_id,governmnetcode,citycode,stationcode")] institute_address institute_address)
        {
            if (ModelState.IsValid)
            {
                db.institute_address.Add(institute_address);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.citycode = new SelectList(db.cities, "citycode", "cityname", institute_address.citycode);
            ViewBag.governmnetcode = new SelectList(db.governmnets, "governmentcode", "governmentname", institute_address.governmnetcode);
            ViewBag.stationcode = new SelectList(db.stations, "stationcode", "stationname", institute_address.stationcode);
            return View(institute_address);
        }

        // GET: institute_address/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            institute_address institute_address = db.institute_address.Find(id);
            if (institute_address == null)
            {
                return HttpNotFound();
            }
            ViewBag.citycode = new SelectList(db.cities, "citycode", "cityname", institute_address.citycode);
            ViewBag.governmnetcode = new SelectList(db.governmnets, "governmentcode", "governmentname", institute_address.governmnetcode);
            ViewBag.stationcode = new SelectList(db.stations, "stationcode", "stationname", institute_address.stationcode);
            return View(institute_address);
        }

        // POST: institute_address/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "institute_adress_id,governmnetcode,citycode,stationcode")] institute_address institute_address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(institute_address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index" , "Institutes");
            }
            ViewBag.citycode = new SelectList(db.cities, "citycode", "cityname", institute_address.citycode);
            ViewBag.governmnetcode = new SelectList(db.governmnets, "governmentcode", "governmentname", institute_address.governmnetcode);
            ViewBag.stationcode = new SelectList(db.stations, "stationcode", "stationname", institute_address.stationcode);
            return View(institute_address );
        }

        // GET: institute_address/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            institute_address institute_address = db.institute_address.Find(id);
            if (institute_address == null)
            {
                return HttpNotFound();
            }
            return View(institute_address);
        }

        // POST: institute_address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            institute_address institute_address = db.institute_address.Find(id);
            db.institute_address.Remove(institute_address);
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
