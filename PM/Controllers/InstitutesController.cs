using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PM.Models;

namespace PM.Controllers
{
    public class InstitutesController : Controller
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: institutes
        public ActionResult Index()
        {
            var institutes = db.institutes.Include(i => i.department).Include(i => i.institute_address).Include(i => i.institute_type);
            return View(institutes.ToList());
        }

        // GET: institutes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            institute institute = db.institutes.Find(id);
            if (institute == null)
            {
                return HttpNotFound();
            }
            return View(institute);
        }

        // GET: institutes/Create
        public ActionResult Create()
        {
            ViewBag.department_id = new SelectList(db.departments, "departementid", "departmentname");

            //ViewBag.stationName = new SelectList(db.stations, "stationcode", "stationname");
            //ViewBag.cityName = new SelectList(db.cities, "citycode", "cityname");
            ViewBag.governmentName = new SelectList(db.governmnets, "governmentcode", "governmentname");
            ViewBag.stationName = new SelectList(new ArrayList { }, "stationcode", "stationname");
            ViewBag.cityName = new SelectList(new ArrayList { }, "citycode", "cityname");
            //ViewBag.governmentName = new SelectList(new ArrayList { }, "governmentcode", "governmentname");


            ViewBag.type_id = new SelectList(db.institute_type, "type_id", "typename");
            return View();
        }

        // POST: institutes/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "institute_id,institutename,telephone , institute_fulladdress,email,type_id,department_id ")] institute institute)
        {




            if (ModelState.IsValid)
            {

                int? governmentName = null;
                int? cityName = null;
                int? stationName = null;

                var shit = Request["cityName"];
                if (Request["governmentName"] != "")
                {
                    governmentName = int.Parse(Request["governmentName"]);
                }
                if (Request["cityName"] != "" && Request["cityName"] != null)
                {
                    cityName = int.Parse(Request["cityName"]);
                }


                if (Request["stationName"] != "" && Request["stationName"] != null)
                {
                    stationName = int.Parse(Request["stationName"]);
                }






                db.institute_address.Add(new institute_address { stationcode = stationName, citycode = cityName, governmnetcode = governmentName });
                db.SaveChanges();


                int lastID = db.institute_address.Max(i => i.institute_adress_id);
                institute.adress_id = lastID; // THE address of the institute is like the PK in the institute_address table


                db.institutes.Add(institute);
                db.SaveChanges(); // to institute_id to be populated first :(






                return RedirectToAction("Index");
            }

            ViewBag.department_id = new SelectList(db.departments, "departementid", "departmentname");
            ViewBag.stationName = new SelectList(db.stations, "stationcode", "stationname");
            ViewBag.cityName = new SelectList(db.cities, "citycode", "cityname");
            ViewBag.governmentName = new SelectList(db.governmnets, "governmentcode", "governmentname");
            ViewBag.type_id = new SelectList(db.institute_type, "type_id", "typename");


            return View(institute);
        }

        // GET: institutes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            institute institute = db.institutes.Find(id);
            if (institute == null)
            {
                return HttpNotFound();
            }
            ViewBag.department_id = new SelectList(db.departments, "departementid", "departmentname", institute.department_id);
            ViewBag.adress_id = new SelectList(db.institute_address, "institute_adress_id", "institute_adress_id", institute.adress_id);
            ViewBag.type_id = new SelectList(db.institute_type, "type_id", "typename", institute.type_id);
            return View(institute);
        }

        // POST: institutes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "institute_id,institutename,institute_fulladdress,telephone,email,type_id,department_id ,adress_id")] institute institute)
        {
            if (ModelState.IsValid)
            {

                db.Entry(institute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.department_id = new SelectList(db.departments, "departementid", "departmentname", institute.department_id);
            ViewBag.adress_id = new SelectList(db.institute_address, "institute_adress_id", "institute_adress_id", institute.adress_id);
            ViewBag.type_id = new SelectList(db.institute_type, "type_id", "typename", institute.type_id);
            return View(institute);
        }

        // GET: institutes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            institute institute = db.institutes.Find(id);
            if (institute == null)
            {
                return HttpNotFound();
            }
            return View(institute);
        }

        // POST: institutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            institute institute = db.institutes.Find(id);
            db.institutes.Remove(institute);
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
