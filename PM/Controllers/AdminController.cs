using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PM.Models;
using PM.ViewModels;
using PagedList;
using System.Data.Entity.Validation;
using System.IO;

namespace PM.Controllers
{
    public class AdminController : Controller
    {

        project_managementEntities1 pm = new project_managementEntities1();
        Jobsection dbVM = new Jobsection();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult add_section()
        {
            //creating a list of type section_type to retrieve all section_type data to let the user select a type
            List<section_type> l = new List<section_type>();
            l = pm.section_type.ToList();
            //converting the list to selectlist to use it in the view and send it in a ViewBag
            ViewBag.sectionTypes = new SelectList(l, "sectiontype_id", "sectiontype");
            ViewBag.addSectionAlert = TempData["addSectionAlert"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult add_section(int sectiontype_id, string sectionname)
        {
            if (sectionname != null)
            {

                //checking for duplication of section name
                //asking the database for the section name the user entered
                var query = (from sec in pm.sections
                             where sec.sectionname == sectionname
                             select sec).FirstOrDefault();
                //if there is duplicated value asking the user to enter another value
                if (query != null)
                {
                    ViewBag.duplicationError = "هذا الاسم قد اضيف سابقا";
                    //sending the selectlist for another submition
                    List<section_type> l = new List<section_type>();
                    l = pm.section_type.ToList();
                    ViewBag.sectionTypes = new SelectList(l, "sectiontype_id", "sectiontype");
                    return View();
                }

                //creating a section object to store the received data in for sending to the database.
                var s = new section
                {
                    sectiontype_id = sectiontype_id,
                    sectionname = sectionname
                };

                //Adding then saving changes by the context.
                pm.sections.Add(s);
                pm.SaveChanges();
                TempData["addSectionAlert"] = "تم اضافة قسم " + sectionname + " بنجاح";
            }

            return RedirectToAction("add_section");
        }

        public ActionResult section_panel(int? pageNumber)

        {

            //making a screen to view all sections ordered alphabetically
            var query = (from sec in pm.sections
                         orderby sec.sectionname
                         select sec).ToList();
            return View(query.ToPagedList(pageNumber ?? 1, 5));

        }

        public ActionResult delete_section(int id)
        {
            //deleting from section_institute table
            List<section_institute> sectionInstituteList = pm.section_institute.Where(x => x.section_id == id).ToList();
            if (sectionInstituteList.Count != 0)
            {

                foreach (var record in sectionInstituteList)
                {
                    pm.section_institute.Remove(record);
                }
            }

            //deleting the selected section from section table
            section s = pm.sections.Find(id);
            pm.sections.Remove(s);
            pm.SaveChanges();
            ViewBag.successfulDelete = "تم مسح القسم";
            return RedirectToAction("section_panel");

        }

        public ActionResult edit_section(int id)
        {

            //this action is navigated from section_panel with id

            //send a viewbag of section_type data to be selected
            List<section_type> l = new List<section_type>();
            l = pm.section_type.ToList();
            ViewBag.sectionTypes = new SelectList(l, "sectiontype_id", "sectiontype");

            //getting the record the user want to edit by id
            section s = pm.sections.Find(id);
            return View(s);

        }
        [HttpPost]
        public ActionResult edit_section(section s)
        {
            if (s != null)
            {


                //checking for duplication
                var query = (from sec in pm.sections
                             where sec.sectionname == s.sectionname && sec.section_id != s.section_id
                             select sec).FirstOrDefault();

                if (query != null)
                {
                    ViewBag.duplicationError = "هذا الاسم قد اضيف سابقا";

                    List<section_type> l = new List<section_type>();
                    l = pm.section_type.ToList();
                    ViewBag.sectionTypes = new SelectList(l, "sectiontype_id", "sectiontype");

                    return View(s);
                }

                //updating the record with the data user altered.
                section updatedS = pm.sections.SingleOrDefault(x => x.section_id == s.section_id);
                if (updatedS != null)
                {
                    updatedS.sectionname = s.sectionname;
                    updatedS.sectiontype_id = s.sectiontype_id;
                    pm.SaveChanges();
                }

                return RedirectToAction("section_panel");
            }

            else
            {
                return RedirectToAction("section_panel");
            }

        }

        public ActionResult section_type_panel(int? pageNumber)
        {

            var query = (from sec in pm.section_type
                         orderby sec.sectiontype
                         select sec).ToList();
            ViewBag.deleteSectionTypeAlert = TempData["deleteSectionTypeAlert"];
            ViewBag.updateSectionTypeSuccess = TempData["updateSectionTypeSuccess"];
            return View(query.ToPagedList(pageNumber ?? 1, 10));


        }

        public ActionResult delete_section_type(int id)
        {

            //deleting from section table
            List<section> sectionList = pm.sections.Where(x => x.sectiontype_id == id).ToList();
            if (sectionList.Count != 0)
            {
                foreach (var record in sectionList)
                {
                    pm.sections.Remove(record);
                }
            }

            //deleting the selected section from section table
            section_type s = pm.section_type.Find(id);
            pm.section_type.Remove(s);

            pm.SaveChanges();

            TempData["deleteSectionTypeAlert"] = "تم مسح نوع القسم " + s.sectiontype + " بنجاح";
            return RedirectToAction("section_type_panel");
        }

        public ActionResult add_section_type()

        {
            ViewBag.addSectionTypeAlert = TempData["addSectionTypeAlert"];
            return View();
        }
        [HttpPost]
        public ActionResult add_section_type(section_type s)
        {
            if (s != null)
            {
                //checking for duplication of section type
                //asking the database for the section type the user entered
                var query = (from sec in pm.section_type
                             where sec.sectiontype == s.sectiontype
                             select sec).FirstOrDefault();

                //if there is duplicated value asking the user to enter another value
                if (query != null)
                {
                    ViewBag.duplicationError = "هذا النوع قد اضيف سابقا";

                    return View();
                }

                pm.section_type.Add(s);

                pm.SaveChanges();

                TempData["addSectionTypeAlert"] = "تم اضافة نوع قسم " + s.sectiontype + " بنجاح";
            }

            return RedirectToAction("add_section_type");
        }


        public ActionResult edit_section_type(int id)

        {

            //this action is navigated from section_type_panel with id

            section_type s = pm.section_type.Find(id);
            return View(s);
        }
        [HttpPost]
        public ActionResult edit_section_type(section_type st)
        {

            if (st != null)
            {

                //checking for duplication
                var query = (from sec in pm.section_type
                             where sec.sectiontype == st.sectiontype && sec.sectiontype_id != st.sectiontype_id
                             select sec).FirstOrDefault();

                if (query != null)
                {
                    ViewBag.duplicationError = "هذا الاسم قد اضيف سابقا";

                    return View(st);
                }

                //updating the record with the data user altered.
                section_type updatedSt = pm.section_type.SingleOrDefault(x => x.sectiontype_id == st.sectiontype_id);
                if (updatedSt != null)
                {
                    updatedSt.sectiontype = st.sectiontype;
                    updatedSt.sectiontype_id = st.sectiontype_id;
                    pm.SaveChanges();
                    TempData["updateSectionTypeSuccess"] = "تم تعديل نوع القسم الى " + st.sectiontype;
                }

                return RedirectToAction("section_type_panel");

            }
            else

            {
                return RedirectToAction("section_type_panel");
            }





        }


        public ActionResult add_project()

        {
            //creating a list of institutes to retrieve all data to let the user select a type
            List<institute> l = new List<institute>();
            l = pm.institutes.ToList();


            //converting the list to selectlist to use it in the view and send it in a ViewBag
            ViewBag.institutes = new SelectList(l, "institute_id", "institutename");
            ViewBag.addProjectSuccess = TempData["addProjectSuccess"];
            return View();

        }

        [HttpPost]
        public ActionResult add_project(project p)
        {
            if (p != null)
            {

                //checking for duplication of project name
                //asking the database for the project name the user entered
                var query = (from proj in pm.projects
                             where proj.projectname == p.projectname
                             select proj).FirstOrDefault();

                //if there is duplicated value asking the user to enter another value
                if (query != null)
                {
                    ViewBag.duplicationError = "هذا الاسم قد اضيف سابقا";

                    //sending the selectlist for submition
                    List<institute> l = new List<institute>();
                    l = pm.institutes.ToList();
                    ViewBag.institutes = new SelectList(l, "institute_id", "institutename");
                    return View();
                }

                pm.projects.Add(p);
                pm.SaveChanges();
                TempData["addProjectSuccess"] = "تم اضافة مشروع " + p.projectname + " بنجاح";


            }
            return RedirectToAction("add_project");
        }


        public ActionResult project_panel(int? pageNumber)
        {

            //making a screen to view all sections ordered alphabetically
            var query = (from proj in pm.projects
                         orderby proj.projectname
                         select proj).ToList();
            ViewBag.deleteProjectSuccess = TempData["deleteProjectSuccess"];
            ViewBag.updateProjectSuccess = TempData["updateProjectSuccess"];
            return View(query.ToPagedList(pageNumber ?? 1, 10));

        }

        public ActionResult delete_project(int id)
        {
            project p = pm.projects.Find(id);

            pm.projects.Remove(p);
            pm.SaveChanges();

            TempData["deleteProjectSuccess"] = "تم مسح مشروع " + p.projectname + " بنجاح";
            return RedirectToAction("project_panel");

        }

        public ActionResult edit_project(int id)
        {

            //
            List<institute> l = new List<institute>();
            l = pm.institutes.ToList();
            ViewBag.institutes = new SelectList(l, "institute_id", "institutename");

            project p = pm.projects.Find(id);

            return View(p);

        }
        [HttpPost]
        public ActionResult edit_project(project p)
        {
            //this if make sure that p is not null and if it is, redirect to "project_panel"
            if (p != null)
            {

                //checking for name duplication 
                var query = (from proj in pm.projects
                             where proj.projectname == p.projectname && proj.project_id != p.project_id
                             select proj).FirstOrDefault();


                if (query != null)
                {

                    ViewBag.duplicationError = "هذا الاسم قد اضيف سابقا";

                    //sending the list of institutes for the dropdownlist
                    List<institute> l = new List<institute>();
                    l = pm.institutes.ToList();
                    ViewBag.institutes = new SelectList(l, "institute_id", "institutename");

                    return View(p);

                }


                //altering the project record
                project updatedP = pm.projects.SingleOrDefault(x => x.project_id == p.project_id);
                if (updatedP != null)
                {
                    updatedP.projectname = p.projectname;
                    updatedP.plannedstartdate = p.plannedstartdate;
                    updatedP.plannedenddate = p.plannedenddate;
                    updatedP.cost = updatedP.cost;
                    updatedP.description = p.description;
                    updatedP.institute_id = p.institute_id;

                    pm.SaveChanges();

                    TempData["updateProjectSuccess"] = "تم تعديل المشروع ";
                }
                return RedirectToAction("project_panel");

            }

            else
            {
                return RedirectToAction("project_panel");
            }

        }

        public ActionResult Cities(int? pageNumber)
        {
            var list = pm.cities.OrderBy(x => x.cityname).ToList();
            ViewBag.deleteCitySuccess = TempData["deleteCitySuccess"];
            ViewBag.updateCitySuccess = TempData["updateCitySuccess"];
            return View(list.ToPagedList(pageNumber ?? 1, 10));
        }
        public ActionResult AddCity()
        {
            List<governmnet> govs = new List<governmnet>();
            govs = pm.governmnets.ToList();
            ViewBag.governmentname = new SelectList(govs, "governmentcode", "governmentname");
            ViewBag.addCitySuccess = TempData["addCitySuccess"];
            return View("");
        }

        [HttpPost]
        public ActionResult AddCity(city model)
        {


            if (model != null)
            {
                var query = (from cit in pm.cities
                             where cit.cityname == model.cityname && cit.governmentcode == model.governmentcode
                             select cit).FirstOrDefault();

                if (query != null)
                {
                    ViewBag.Duplicate = "هذا الاسم قد اضيف سابقا";
                    List<governmnet> govs = new List<governmnet>();
                    govs = pm.governmnets.ToList();
                    ViewBag.governmentname = new SelectList(govs, "governmentcode", "governmentname");
                    return View("AddCity");
                }

                var cityy = new city
                {
                    citycode = model.citycode,
                    cityname = model.cityname,
                    governmentcode = model.governmentcode,
                    governmnet = model.governmnet
                };


                pm.cities.Add(cityy);
                pm.SaveChanges();
                TempData["addSCitySuccess"] = "تم اضافة مدينة " + model.cityname + " بنجاح";

                return RedirectToAction("Cities");



            }
            TempData["addCitySuccess"] = "ادخل اسم المدينة صحيح ";
            return RedirectToAction("Addcity");
        }
        public ActionResult DeleteCity(int id)
        {

            try
            {

                var obj = pm.cities.Find(id);
                var stations = pm.stations.Where(x => x.citycode == obj.citycode).ToList();



                pm.stations.RemoveRange(stations);
                pm.cities.Remove(obj);
                pm.SaveChanges();
                return RedirectToAction("Cities");
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public ActionResult EditCity(int id)
        {

            List<governmnet> gov = new List<governmnet>();
            gov = pm.governmnets.ToList();
            ViewBag.governmentname = new SelectList(gov, "governmentcode", "governmentname");

            //getting the record the user want to edit by id
            city s = pm.cities.Find(id);
            return View(s);
        }
        [HttpPost]
        public ActionResult EditCity(city cityy)
        {

            if (cityy != null)
            {
                var query = (from cit in pm.cities
                             where cit.cityname == cityy.cityname && cit.citycode != cityy.citycode && cit.governmentcode == cityy.governmentcode
                             select cit).FirstOrDefault();
                if (query != null)
                {
                    ViewBag.Duplicate = "هذا الاسم قد اضيف سابقا";
                    List<governmnet> gov = new List<governmnet>();
                    gov = pm.governmnets.ToList();
                    ViewBag.governmentname = new SelectList(gov, "governmentcode", "governmentname");
                    return View(cityy);
                }

                city updatedS = pm.cities.SingleOrDefault(x => x.citycode == cityy.citycode);
                if (updatedS != null)
                {
                    updatedS.cityname = cityy.cityname;
                    updatedS.governmentcode = cityy.governmentcode;
                    pm.SaveChanges();
                    TempData["updateCitySuccess"] = "تم التعديل";
                }
                return RedirectToAction("Cities");
            }

            else
            {
                return RedirectToAction("Cities");
            }

        }

        public ActionResult Governments(int? pageNumber)
        {
            var list = pm.governmnets.OrderBy(x => x.governmentname).ToList();
            ViewBag.deletegovernmentSuccess = TempData["deletegovernmentSuccess"];
            ViewBag.updategovernmentSuccess = TempData["updategovernmentSuccess"];
            return View(list.ToPagedList(pageNumber ?? 1, 10));
        }

        public ActionResult AddGovernment()
        {
            ViewBag.addGovernmentSuccess = TempData["addGovernmentSuccess"];
            return View("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddGovernment(governmnet model)
        {
            var governments = pm.governmnets.Where(x => x.governmentname == model.governmentname).ToList();
            if (ModelState.IsValid)
            {

                governmnet obj = new governmnet();
                if (governments.Count > 0)
                {
                    ViewBag.Duplicate = "اسم المخافظة " + model.governmentname + " موجود بالفعل";
                    return View("AddGovernment");
                }

                obj.governmentcode = model.governmentcode;
                obj.governmentname = model.governmentname;
                pm.governmnets.Add(obj);
                pm.SaveChanges();
                TempData["addGovernmentSuccess"] = "تم اضافة مركز " + model.governmentname + " بنجاح";
                return RedirectToAction("Governments");




            }
            TempData["addGovernmentSuccess"] = "ادخل اسم المحافظة صحيح ";
            return RedirectToAction("AddGovernment");
        }


        public ActionResult DeleteGovernment(int id)
        {

            try
            {

                var obj = pm.governmnets.Find(id);
                var cities = pm.cities.Where(x => x.governmentcode == obj.governmentcode).ToList();
                var citiescode = pm.cities.Where(x => x.governmentcode == obj.governmentcode).Distinct().ToList();
                foreach (var city in citiescode)
                {
                    var stations = pm.stations.Where(x => x.citycode == city.citycode).ToList();
                    pm.stations.RemoveRange(stations);
                }

                pm.cities.RemoveRange(cities);
                pm.governmnets.Remove(obj);
                pm.SaveChanges();
                return RedirectToAction("Governments");
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public ActionResult EditGovernment(int id)
        {
            governmnet gov = pm.governmnets.Find(id);
            ViewBag.addGovernmentSuccess = TempData["addGovernmentSuccess"];
            return View(gov);
        }
        [HttpPost]
        public ActionResult EditGovernment(governmnet gov)
        {
            if (gov != null)
            {
                var query = (from g in pm.governmnets
                             where g.governmentname == gov.governmentname && g.governmentcode != gov.governmentcode
                             select g).FirstOrDefault();

                if (query != null)
                {
                    ViewBag.Duplicate = "هذا الاسم قد اضيف سابقا";
                    return View(gov);
                }
                governmnet updatedgov = pm.governmnets.SingleOrDefault(x => x.governmentcode == gov.governmentcode);
                if (updatedgov != null)
                {
                    updatedgov.governmentname = gov.governmentname;

                    pm.SaveChanges();
                }

                return RedirectToAction("Governments");
            }
            else
            {
                return RedirectToAction("Governments");
            }

        }



        public ActionResult Stations(int? pageNumber)
        {
            var list = pm.stations.OrderBy(x => x.stationname).ToList();
            ViewBag.deleteStationSuccess = TempData["deleteStationSuccess"];
            ViewBag.updateStationSuccess = TempData["updateStationSuccess"];
            return View(list.ToPagedList(pageNumber ?? 1, 10));
        }
        public ActionResult AddStation()
        {
            List<city> cities = new List<city>();
            cities = pm.cities.ToList();
            ViewBag.cityname = new SelectList(cities, "citycode", "cityname");
            ViewBag.addStationSuccess = TempData["addStationSuccess"];
            return View();
        }
        [HttpPost]
        public ActionResult AddStation(station model)
        {
            if (model != null)
            {
                var query = (from st in pm.stations
                             where st.stationname == model.stationname && st.citycode == model.citycode
                             select st).FirstOrDefault();


                if (query != null)
                {

                    ViewBag.Duplicate = "اسم المركز" + model.stationname + "موجود بالفعل";
                    List<city> cit = new List<city>();
                    cit = pm.cities.ToList();
                    ViewBag.cityname = new SelectList(cit, "citycode", "cityname");
                    return View("AddStation");
                }

                var s = new station
                {
                    stationname = model.stationname,
                    citycode = model.citycode
                };

                pm.stations.Add(s);
                pm.SaveChanges();
                TempData["addStationSuccess"] = "تم اضافة مركز " + model.stationname + " بنجاح";

            }
            else
            {
                TempData["addStationSuccess"] = "ادخل اسم المركز صحيح ";

            }
            return RedirectToAction("Addstation");

        }

        public ActionResult DeleteStation(int id)
        {

            try
            {

                var obj = pm.stations.Find(id);

                pm.stations.Remove(obj);
                pm.SaveChanges();
                TempData["addStationSuccess"] = "تم مسح مركز " + obj.stationname + " بنجاح";
                return RedirectToAction("Stations");
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public ActionResult EditStation(int id)
        {

            List<city> cit = new List<city>();
            cit = pm.cities.ToList();
            ViewBag.cityname = new SelectList(cit, "citycode", "cityname");

            //getting the record the user want to edit by id
            station s = pm.stations.Find(id);
            return View(s);
        }
        [HttpPost]
        public ActionResult EditStation(station s)
        {


            if (s != null)
            {
                var query = (from st in pm.stations
                             where st.stationname == s.stationname && st.stationcode != s.stationcode && s.citycode == st.citycode
                             select st).FirstOrDefault();
                if (query != null)
                {
                    List<city> cit = new List<city>();
                    cit = pm.cities.ToList();
                    ViewBag.Duplicate = "هذا الاسم قد اضيف سابقا";
                    ViewBag.cityname = new SelectList(cit, "citycode", "cityname");
                    return View(s);
                }
                station updatedst = pm.stations.SingleOrDefault(x => x.stationcode == s.stationcode);

                if (updatedst != null)
                {
                    updatedst.stationname = s.stationname;
                    updatedst.citycode = s.citycode;
                    pm.SaveChanges();

                }
                return RedirectToAction("Stations");
            }
            else
            {

                return RedirectToAction("Stations");
            }




        }

        public ActionResult Jobs(int? pageNumber)
        {

            //dbVM.jobs = pm.jobs.OrderBy(x => x.jobname).ToList();
            var y = (from x in pm.jobs join s in pm.job_section on x.job_id equals s.job_id join w in pm.sections on s.section_id equals w.section_id orderby x.jobname select new Jobsection { jobs = x, sections = w });
            ViewBag.deleteJobSuccess = TempData["deleteJobSuccess"];
            ViewBag.updateJobSuccess = TempData["updateJobSuccess"];
            return View(y.ToPagedList(pageNumber ?? 1, 10));


        }

        public ActionResult AddJob()
        {
            dbVM.jobs2 = pm.jobs.ToList();
            dbVM.sections2 = pm.sections.ToList();
            ViewBag.sectionnames = new SelectList(dbVM.sections2, "section_id", "sectionname");

            ViewBag.jobnames = new SelectList(dbVM.jobs2, "job_id", "jobname");
            ViewBag.addJobSuccess = TempData["addJobSuccess"];

            return View(dbVM);
        }

        [HttpPost]
        public ActionResult AddJob(Jobsection model)
        {
            model.sections = (from sec in pm.sections where sec.section_id == model.section_id select sec).FirstOrDefault();

            if (model != null)
            {
                var query = (from job in pm.jobs join sec in pm.job_section on job.job_id equals sec.job_id
                             where job.jobname == model.jobs.jobname && job.followupcode == model.jobs.followupcode && sec.section_id == model.section_id
                             select job).FirstOrDefault();


                if (query != null)
                {
                    ViewBag.Duplicate = "هذا الاسم قد اضيف سابقا";
                    dbVM.jobs2 = pm.jobs.ToList();
                    dbVM.sections2 = pm.sections.ToList();
                    ViewBag.sectionnames = new SelectList(dbVM.sections2, "section_id", "sectionname");

                    ViewBag.jobnames = new SelectList(dbVM.jobs2, "job_id", "jobname");
                    return View("AddJob");
                }


                job obj = new job();
                obj.jobname = model.jobs.jobname;
                obj.followupcode = model.jobs.followupcode;
                

                pm.jobs.Add(obj);
                pm.job_section.Add(new job_section { job_id = model.jobs.job_id, section_id = model.section_id });
                pm.SaveChanges();
                

            }
            
            return RedirectToAction("Jobs");

        }

        public ActionResult DeleteJob(int id)
        {

            try
            {

                dbVM.jobs = pm.jobs.Find(id);


                pm.job_section.Remove(pm.job_section.Where(x => x.job_id == dbVM.jobs.job_id).FirstOrDefault());
                pm.jobs.Remove(dbVM.jobs);
                pm.SaveChanges();
                TempData["addStationSuccess"] = "تم مسح وظيفة " + dbVM.jobs.jobname + " بنجاح";
                return RedirectToAction("Jobs");
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public ActionResult EditJob(int id)
        {


            dbVM.jobs2 = pm.jobs.ToList();
            dbVM.sections2 = pm.sections.ToList();
            ViewBag.sectionnames = new SelectList(dbVM.sections2, "section_id", "sectionname");

            ViewBag.jobnames = new SelectList(dbVM.jobs2, "job_id", "jobname");
            ViewBag.addJobSuccess = TempData["addJobSuccess"];
            dbVM.jobs = pm.jobs.Where(x => x.job_id == id).FirstOrDefault();
            return View(dbVM);


        }
        [HttpPost]
        public ActionResult EditJob(Jobsection model)
        {
            
            if (model != null)
            {
                model.sections = (from sec in pm.sections where sec.section_id == model.section_id select sec).FirstOrDefault();
                var query = (from job in pm.jobs join sec in pm.job_section on job.job_id equals sec.job_id
                             where job.jobname == model.jobs.jobname && job.followupcode== model.jobs.followupcode  && sec.section_id == model.section_id
                             select job).FirstOrDefault();
                

                if (query != null)
                {

                    ViewBag.Duplicate = "هذا الاسم قد اضيف سابقا";
                    dbVM.jobs2 = pm.jobs.ToList();
                    dbVM.sections2 = pm.sections.ToList();
                    ViewBag.sectionnames = new SelectList(dbVM.sections2, "section_id", "sectionname");
                    ViewBag.jobnames = new SelectList(dbVM.jobs2, "job_id", "jobname");
                    return View(model);
                }

                Jobsection updatedmodel = new Jobsection();
                updatedmodel.jobs = pm.jobs.SingleOrDefault(x => x.job_id == model.jobs.job_id);
                updatedmodel.sections = pm.sections.SingleOrDefault(x => x.section_id == model.section_id);
                if (updatedmodel != null)
                {
                    updatedmodel.jobs.jobname = model.jobs.jobname;
                    updatedmodel.sections.sectionname = model.sections.sectionname;
                    updatedmodel.jobs.followupcode = model.jobs.followupcode;
                    pm.SaveChanges();
                    TempData["updateStationSuccess"] = "تم التعديل";
                }
                else
                {
                    TempData["updateStationSuccess"] = "لا يمكن تعديله";

                }

                


            }
            return RedirectToAction("Jobs");

        }

        public ActionResult show_project_stage_panel()
		{
            //exists only for showing the view.
            ViewBag.duplicationError = TempData["duplicationError"];
            ViewBag.updateStageSuccessfull = TempData["updateStageSuccessfull"];
            return View();
		}

        public ActionResult project_stages_panel()
		{
            //this action for sending data to datatable table using Ajax
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                string columnIndex = Request.Form.GetValues("order[0][column]").FirstOrDefault(); 
                string sortColumn = Request.Form.GetValues($"columns[{columnIndex}][data]").FirstOrDefault();
                //var sortColumn = Request.Form.GetValues("columns[" + columnName + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data    
                var query = (from ps in pm.project_stage
                                    select ps);


                Dictionary<string, Func<project_stage, object>> field_mapper = new Dictionary<string, Func<project_stage, object>>()
                    {
                        {"stage_name", t => t.stage_name},
                    };

                if (sortColumnDir == "asc")
                {
                    query.OrderBy(field_mapper[sortColumn]).Skip(skip).Take(pageSize); 
                   // hack of the day :)
                }
                else
                {
                    query.OrderByDescending(field_mapper[sortColumn]);
                 
                }
                
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => m.stage_name.Contains(searchValue));
                }
                
                //total number of rows count     
                recordsTotal = query.Count();
                //Paging     
                var data = query.ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult add_stage(string  new_stage)
		{
            //it came to me validated in the client side.
            if(new_stage != null)
			{
                //checking if the new name already exists in the database or not
                var query = (from p in pm.project_stage
                             where p.stage_name == new_stage
                             select p).FirstOrDefault();
                if (query != null)
                {
                    TempData["duplicationError"] = "هذا الاسم قد اضيف سابقا";
                    return RedirectToAction("show_project_stage_panel");
                }


                project_stage ps = new project_stage { stage_name = new_stage };
                pm.project_stage.Add(ps);
                pm.SaveChanges();
                TempData["addStageSuccessfull"] = "تم اضافة اسم مرحلة " + new_stage + " بنجاح";

                return RedirectToAction("show_project_stage_panel");

            }
            else
			{
                return RedirectToAction("show_project_stage_panel");

            }
            

        }

        
        public ActionResult delete_stage(int id)
		{
            project_stage ps = pm.project_stage.Find(id);
            pm.project_stage.Remove(ps);
            pm.SaveChanges();
            TempData["deleteSuccessfull"] = "تم مسح الاسم " + ps.stage_name + "بنجاح";
            return RedirectToAction("project_stages_panel");
		}

        public ActionResult edit_stage(int id, string stage_update)
		{
            if(stage_update != null)
			{
                var query = (from p in pm.project_stage
                             where p.stage_name == stage_update && p.stage_id != id
                             select p).FirstOrDefault();
                if(query != null)
				{
					TempData["duplicationError"] = "هذا الاسم قد اضيف سابقا";
                    return RedirectToAction("show_project_stage_panel");
                }

                //updating the record with the data user altered.
                project_stage updatedStage = pm.project_stage.SingleOrDefault(x => x.stage_id == id);
                if (updatedStage != null)
                {
                    updatedStage.stage_name = stage_update;

                    pm.SaveChanges();
                    TempData["updateStageSuccessfull"] = "تم تعديل اسم المرحلة الى " + stage_update;
                }
                return RedirectToAction("show_project_stage_panel");
            }
			else
			{
                return RedirectToAction("show_project_stage_panel");
            }
		}



        public ActionResult project_main_page()
		{
            ViewBag.projectCreatedSuccessfully = TempData["projectCreatedSuccessfully"];
            ViewBag.duplicationError = TempData["duplicationError"];
            return View();
		}

        [HttpPost]
        public ActionResult create_project(project p,HttpPostedFileBase attachment)
		{
            var query = (from proj in pm.projects
                         where proj.projectname == p.projectname
                         select proj).FirstOrDefault();
            //checking for name duplication
            if (query != null)
            {
                TempData["duplicationError"] = "هذا الاسم قد تم استخدامه سابقا";
                return RedirectToAction("project_main_page");
            }
            //static assigning will be handled later....
            p.project_stage_id = 1;
            p.client = 1;
            pm.projects.Add(p);
            pm.SaveChanges();

            var projectWithId = (from proj in pm.projects
                                 where proj.projectname == p.projectname
                                 select proj).FirstOrDefault();

            if (attachment != null)
            {
                //saving the file in the appropriate folder
                 
                string path = Server.MapPath("~/project_attachments");
                string filename = Path.GetFileName(attachment.FileName);
                string full_path = Path.Combine(path, filename);
                attachment.SaveAs(full_path);

                //adding a project attachment
                attachemnt a = new attachemnt { attachment_name = attachment.FileName };
                pm.attachemnts.Add(a);
                pm.SaveChanges();

                //getting the id that is automatically generated in db of that attachment
                var attachmentQuery = (from attach in pm.attachemnts
                                       where attach.attachment_name == attachment.FileName
                                       select attach).FirstOrDefault();

                project_attachment pa = new project_attachment { project_id = projectWithId.project_id, attachment_id = attachmentQuery.attachment_id };
                pm.project_attachment.Add(pa);
                pm.SaveChanges();
            }
            TempData["projectCreatedSuccessfully"] = "تم انشاء المشروع بنجاح";
            return RedirectToAction("project_main_page");
		}

    }
}