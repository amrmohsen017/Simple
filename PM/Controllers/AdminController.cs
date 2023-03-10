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
                         select new sectionTypeView
                         {
                             sectiontype_id = sec.sectiontype_id,
                             sectiontype = sec.sectiontype
                         }).ToList();

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
            sectionTypeView s_2 = new sectionTypeView
            {
                sectiontype = s.sectiontype,
                sectiontype_id = s.sectiontype_id
            };

            return View(s_2);
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
                var query = from ps in pm.project_stage
                            select new stagesView
                            {
                                stage_id = ps.stage_id,
                                stage_name = ps.stage_name
                            };
                            


                Dictionary<string, Func<stagesView, object>> field_mapper = new Dictionary<string, Func<stagesView, object>>()
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
            projectsPanelView ppv = new projectsPanelView();
            ViewBag.projectCreatedSuccessfully = TempData["projectCreatedSuccessfully"];
            ViewBag.duplicationError = TempData["duplicationError"];
            return View(ppv);
		}


        [HttpPost]
        public ActionResult create_project(project p) //HttpPostedFileBase attachment)
		{
            try
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
                //handling if the user didnt enter a stage.
                if (p.project_stage_id == null)
                    p.project_stage_id = 1;
                p.client = 6;
                p.project_status = 1;
                pm.projects.Add(p);
                pm.SaveChanges();

                //var projectWithId = (from proj in pm.projects
                //                     where proj.projectname == p.projectname
                //                     select proj).FirstOrDefault();

                //if (attachment != null)
                //{
                //    //saving the file in the appropriate folder

                //    string path = Server.MapPath("~/project_attachments");
                //    string filename = Path.GetFileName(attachment.FileName);
                //    string full_path = Path.Combine(path, filename);
                //    attachment.SaveAs(full_path);

                //    //adding a project attachment
                //    attachemnt a = new attachemnt { attachment_name = attachment.FileName };
                //    pm.attachemnts.Add(a);
                //    pm.SaveChanges();

                //    //getting the id that is automatically generated in db of that attachment
                //    var attachmentQuery = (from attach in pm.attachemnts
                //                           where attach.attachment_name == attachment.FileName
                //                           select attach).FirstOrDefault();

                //    project_attachment pa = new project_attachment { project_id = projectWithId.project_id, attachment_id = attachmentQuery.attachment_id };
                //    pm.project_attachment.Add(pa);
                //    pm.SaveChanges();

                //}
                TempData["projectCreatedSuccessfully"] = "تم انشاء المشروع بنجاح";

                //adding project creation log
                #region adding project creation to the log

                string log_text = "Project with id = " + p.project_id.ToString() + " and name = " + p.projectname + " has been created successfully";
                DateTime now = DateTime.Now;
                log new_log = new log
                {
                    log_text = log_text,
                    log_date = now
                };
                pm.logs.Add(new_log);
                pm.SaveChanges();
                
                var get_new_log = (from l in pm.logs
                                   where l.log_text == log_text
                                   select l).FirstOrDefault();

                project_log pl = new project_log
                {
                    log_id = get_new_log.log_id,
                    project_id = p.project_id
                };
                pm.project_log.Add(pl);
                pm.SaveChanges();
                #endregion

                return RedirectToAction("project_main_page");
            }
            catch
			{
                throw;
			}
		}

        public ActionResult show_attachment_list()
		{

            return View();
		}
        public ActionResult fill_attachment_list()
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

                //Getting all Customer data

                var query = (from p in pm.projects
                             join pa in pm.project_attachment on p.project_id equals pa.project_id
                             join a in pm.attachemnts on pa.attachment_id equals a.attachment_id
                             select new projectAttchment
                             {
                                 project_name = p.projectname,
                                 project_id = p.project_id,
                                 attachment_id=a.attachment_id,
                                 attachment_name = a.attachment_name
                             }) ;

                //var query = pm.attachemnts; 

                Dictionary<string, Func<projectAttchment, object>> field_mapper = new Dictionary<string, Func<projectAttchment, object>>()
                    {
                        {"attachment_name", t => t.attachment_name}
                    };

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => m.attachment_name.Contains(searchValue) || m.project_name.Contains(searchValue));
                  
                }


             
                if (sortColumnDir == "asc")
                {
                    var dataa = query.OrderBy(field_mapper[sortColumn]).Skip(skip).Take(pageSize);
                    // hack of the day :)
                    //recordsTotal = query.Count();
                    //return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataa });
                }
                else
                {
                    var dataa = query.OrderByDescending(field_mapper[sortColumn]);
                    //recordsTotal = query.Count();
                    //return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataa });

                }



				//total number of rows count     
				recordsTotal = query.Count();
                ////Paging     
                var data = query.ToList();
               
				////Returning Json Data    
				return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

			}
            catch (Exception)
            {
                throw;
            }

        }

        public FileResult download_attachment(string attachment_name)
		{
            string path = Server.MapPath("~/project_attachments");
            string filename = Path.GetFileName(attachment_name);
            string fullpath = Path.Combine(path, filename);
            return File(fullpath, attachment_name,attachment_name);

		}

        public ActionResult show_gross_margins()
		{

            return View();
		}

        public ActionResult fill_gross_margins()
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

                //Getting all Customer data

                var query = (from g in pm.gross_marign
                             select new grossMarginView
                             {
                                 project_name = g.project.projectname,
                                 project_id = g.project.project_id,
                                 Amount = g.Amount,
                                 quantity = g.quantity,
                                 gross_margin_id = g.gross_marign_id,
                                 description = g.description,
                                 gross_date = g.gross_date,
                                 funder = g.user.username,
                                 gross_margin_typename = g.gross_marign_type.gross_marign_typename,
                                 gross_type = g.gross_type,
                                 user_associated = g.user_associated
                             });


                Dictionary<string, Func<grossMarginView, object>> field_mapper = new Dictionary<string, Func<grossMarginView, object>>()
                    {
                        {"project_name", t => t.project_name},
                        {"Amount", t => t.Amount},
                        {"quantity", t => t.quantity},
                        {"gross_date", t => t.gross_date},
                        {"funder", t => t.funder},
                        {"gross_margin_typename", t => t.gross_margin_typename}

                    };

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => m.gross_margin_typename.Contains(searchValue) || m.project_name.Contains(searchValue)
                    || m.funder.Contains(searchValue) || m.description.Contains(searchValue));

                }


                IEnumerable<grossMarginView> dataa;
                if (sortColumnDir == "asc")
                {
                     dataa = query.OrderBy(field_mapper[sortColumn]).Skip(skip).Take(pageSize);
                    // hack of the day :)
                    //recordsTotal = query.Count();
                    //return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataa });
                }
                else
                {
                     dataa = query.OrderByDescending(field_mapper[sortColumn]);
                    //recordsTotal = query.Count();
                    //return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataa });

                }



                //total number of rows count     
                recordsTotal = dataa.Count();
                ////Paging     
                var data = dataa.ToList();

                ////Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
		}

        public ActionResult show_gross_types()
		{
            return View();
		}

        public ActionResult fill_gross_types()
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

                //Getting all gross_marign_type data
                //var main_query = from gt in pm.gross_marign_type
                                 //select gt;


                var query = from gt in pm.gross_marign_type
                            select new grossTypes
                            {
                                id = gt.id,
                                gross_marign_typename = gt.gross_marign_typename,
                                number_of_marigns_associated = gt.gross_marign.Count()
                             };




                Dictionary<string, Func<grossTypes, object>> field_mapper = new Dictionary<string, Func<grossTypes, object>>()
                    {
                        {"gross_marign_typename", t => t.gross_marign_typename}

                    };

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => m.gross_marign_typename.Contains(searchValue));

                }


                IEnumerable<grossTypes> dataa;
                if (sortColumnDir == "asc")
                {
                    dataa = query.OrderBy(field_mapper[sortColumn]).Skip(skip).Take(pageSize);
                    // hack of the day :)
                    //recordsTotal = query.Count();
                    //return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataa });
                }
                else
                {
                    dataa = query.OrderByDescending(field_mapper[sortColumn]);
                    //recordsTotal = query.Count();
                    //return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataa });

                }



                //total number of rows count     
                recordsTotal = dataa.Count();
                ////Paging     
                var data = dataa.ToList();

                ////Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult show_logs()
		{
            return View();
		}

        public ActionResult fill_logs()
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


                var query = from l in pm.logs
                            select new logView
                            {
                                log_id = l.log_id,
                                log_text = l.log_text,
                                log_date = l.log_date.ToString()
                            };




                Dictionary<string, Func<logView, object>> field_mapper = new Dictionary<string, Func<logView, object>>()
                    {
                        {"log_date", t => t.log_date}

                    };

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(m => m.log_text.Contains(searchValue) || m.log_id.ToString().Contains(searchValue) || m.log_date.ToString().Contains(searchValue));

                }


                IEnumerable<logView> dataa;
                if (sortColumnDir == "asc")
                {
                    dataa = query.OrderBy(field_mapper[sortColumn]).Skip(skip).Take(pageSize);
                    // hack of the day :)
                }
                else
                {
                    dataa = query.OrderByDescending(field_mapper[sortColumn]);
                }



                //total number of rows count     
                recordsTotal = dataa.Count();
                ////Paging     
                var data = dataa.ToList();

                ////Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult add_more_project_data(int project_id)
		{
			try
			{
                pm.Configuration.ProxyCreationEnabled = false;
                var p = (from x in pm.projects
                         where x.project_id == project_id
                         select new projectMoreData
                         {
                             projectname = x.projectname,
                             description = x.description,
                             client = x.client,
                             project_id = x.project_id
                         }).FirstOrDefault();



                return View(p);
            }
			catch
			{
                throw;
			}
            
		}

        [HttpPost]
        public ActionResult add_more_project_data(projectMoreData pmd, HttpPostedFileBase[] files)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    //converting the json to gross_marign list
                    var gm_list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<gross_marign>>(pmd.gm);

                    #region adding the project first
                    var query = (from proj in pm.projects
                                 where proj.projectname == pmd.projectname && proj.project_id != pmd.project_id
                                 select proj).FirstOrDefault();

                    //checking for name duplication
                    if (query != null)
                    {
                        pmd.duplication = "duplicated";
                        return View(pmd);
                    }


                    var p = (from proj in pm.projects
                             where proj.project_id == pmd.project_id
                             select proj).FirstOrDefault();


                    p.projectname = pmd.projectname;
                    p.plannedstartdate = pmd.plannedstartdate;
                    p.plannedenddate = pmd.plannedenddate;
                    p.description = pmd.description;
                    p.cost = pmd.cost;
                    p.deadline_date = pmd.deadline_date;
                    p.institute_id = pmd.institute_id;
                    p.project_status = pmd.project_status;
                    p.project_stage_id = pmd.project_stage_id;



                    //handling if the user didnt enter a stage.                     
                    if (pmd.project_stage_id == null)
                        p.project_stage_id = 1;
                    //static assigning will be handled later....
                    p.client = 6;
                    pm.SaveChanges();
                    #endregion


                    #region second adding attachments
                    if (files[0] != null)
                    {
                        foreach (HttpPostedFileBase file in files)
                        {

                            string path = Server.MapPath("~/project_attachments");
                            string filename = Path.GetFileName(file.FileName);
                            string full_path = Path.Combine(path, filename);
                            var file_redudency = pm.attachemnts.FirstOrDefault(x => x.attachment_name == file.FileName);
                            if (file_redudency == null)
                            {
                                //this if for not saving a file more than once but we need file size to make sure it is the right file
                                file.SaveAs(full_path);
                            }



                            //adding a project attachment
                            attachemnt a = new attachemnt
                            {
                                attachment_name = file.FileName,
                                attachment_path = full_path
                            };
                            pm.attachemnts.Add(a);
                            pm.SaveChanges();

                            //getting the id that is automatically generated in db of that attachment
                            //var attachmentQuery = (from attach in pm.attachemnts
                            //					   where attach.attachment_name == file.FileName
                            //					   select attach).FirstOrDefault();

                            //if(attachmentQuery != null)
                            //{
                            //}

                            project_attachment pa = new project_attachment { project_id = p.project_id, attachment_id = a.attachment_id };
                            pm.project_attachment.Add(pa);
                            pm.SaveChanges();




                        }
                    }
                    #endregion

                    DateTime now = DateTime.Now; //taking time right after the saving in the database to be accurate as possible.

                    #region third adding gross_marign

                    //looping over the gross_marign list
                    for (int i = 0; i < gm_list.Count(); i++)
                    {
                        gm_list[i].project_id = pmd.project_id;
                        pm.gross_marign.Add(gm_list[i]);
                    }
                    pm.SaveChanges();





                    #endregion

                    #region adding project update to the log

                    string log_text = $"project with name {pmd.projectname} and id {pmd.project_id} has been updated FROM name: {pmd.projectname} TO {p.projectname}, " +
                        $"plannedstartdate: {pmd.plannedstartdate} TO {p.plannedstartdate}, plannedenddate: {pmd.plannedenddate} TO {p.plannedenddate}, description:{pmd.description} TO {p.description}, " +
                        $"cost: {pmd.cost} TO {p.cost}, deadline_date: {pmd.deadline_date} TO  {p.deadline_date}" +
                        $"institute_id = {pmd.institute_id} TO {p.institute_id}, project_status: {pmd.project_status} TO {p.project_status},  project_stage_id = {pmd.project_stage_id} TO {p.project_stage_id}";

                    for (int l = 0; l < gm_list.Count(); l++)
                    {
                        log_text = log_text + $", gross_marign_id = {gm_list[l].gross_marign_id}, gross_description = {gm_list[l].description}, gross_date = {gm_list[l].gross_date}" +
                                $", quantity = {gm_list[l].quantity}, Amount = {gm_list[l].Amount}, type = {gm_list[l].gross_type}, user_associated = {gm_list[l].user_associated}";
                    }

                    log new_log = new log
                    {
                        log_text = log_text,
                        log_date = now
                    };
                    pm.logs.Add(new_log);
                    pm.SaveChanges();

                    //var get_new_log = (from l in pm.logs
                    //                   where l.log_text == log_text
                    //                   select l).FirstOrDefault();

                    project_log pl = new project_log
                    {
                        log_id = new_log.log_id,
                        project_id = p.project_id
                    };
                    pm.project_log.Add(pl);
                    pm.SaveChanges();
                    #endregion



                    return RedirectToAction("project_main_page");
                }
				else
				{
                    return View(pmd);
				}
            }
            catch
			{
                throw;
			}
            
		}

        public ActionResult project_logs(int project_id)
		{
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
                var query = from l in pm.project_log
                            where l.project_id==project_id
                            select new logView
                            {
                                log_id = l.log.log_id,
                                log_text = l.log.log_text,
                                log_date=l.log.log_date.ToString()
                            };



                Dictionary<string, Func<logView, object>> field_mapper = new Dictionary<string, Func<logView, object>>()
                    {
                        {"log_text", t => t.log_text},
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
                    query = query.Where(m => m.log_text.Contains(searchValue));
                }

                //total number of rows count     
                recordsTotal = query.Count();
                //Paging     
                var data = query.ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch(Exception)
			{
                throw;
			}
		}

        public ActionResult project_gross(int project_id)
		{
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
                var query = from g in pm.gross_marign
                            where g.project_id == project_id
                            select new grossMarginView
                            {
                                gross_margin_id = g.gross_marign_id,
                                description = g.description,
                                Amount = g.Amount,
                                quantity=g.quantity,
                                gross_date=g.gross_date,
                                user_associated=g.user_associated,
                                gross_type=g.gross_type,
                                gross_margin_typename=g.gross_marign_type.gross_marign_typename,
                                funder=g.user.username,
                                gross_date_string= g.gross_date.ToString()
                            };



                Dictionary<string, Func<grossMarginView, object>> field_mapper = new Dictionary<string, Func<grossMarginView, object>>()
                    {
                        {"description", t => t.description},
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
                    query = query.Where(m => m.description.Contains(searchValue));
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


        public ActionResult project_attachment(int project_id)
		{
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
                var query = from a in pm.project_attachment
                            where a.project_id == project_id
                            select new projectAttchment
                            {
                                attachment_path = a.attachemnt.attachment_path,
                                attachment_name = a.attachemnt.attachment_name,
                                attachment_id = a.attachemnt.attachment_id
                            };



                Dictionary<string, Func<projectAttchment, object>> field_mapper = new Dictionary<string, Func<projectAttchment, object>>()
                    {
                        {"attachment_name", t => t.attachment_name},
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
                    query = query.Where(m => m.attachment_name.Contains(searchValue));
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

        public ActionResult project_updates(int project_id)
		{
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
                var query = from u in pm.project_updates
                            where u.project_id == project_id
                            select new projectUpdatesView
                            {
                                update_id = u.update_id,
                                description = u.update_description,
                                update_name = u.update_name,
                                update_date = u.update_date,
                                update_date_string = u.update_date.ToString(),
                                status = u.update_status,
								status_name = u.status.status_name,
								progress = u.update_progress,
                                updater = u.update_author_id,
                                updater_name = u.user.username,
                                project_id = u.project_id
                            };



                Dictionary<string, Func<projectUpdatesView, object>> field_mapper = new Dictionary<string, Func<projectUpdatesView, object>>()
                    {
                        {"update_name", t => t.update_name},
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
                    query = query.Where(m => m.description.Contains(searchValue));
                }

                //total number of rows count     
                recordsTotal = query.Count();
                //Paging     
                var data = query.ToList();
                //Returning Json Data    
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch(Exception)
			{
                throw;
			}
		}

        public ActionResult edit_project_fields(int project_id,string value, string field_name)
		{

			try
			{
                var p = (from proj in pm.projects
                         where proj.project_id == project_id
                         select proj).FirstOrDefault();
                string old_data = null;
                switch (field_name)
                {
                    case "description":
                        old_data = p.description;
                        p.description = value;
                        break;
                    case "plannedstartdate":
                        old_data = p.plannedstartdate.ToString();
                        p.plannedstartdate = Convert.ToDateTime(value);                       
                        break;
                    case "plannedenddate":
                        old_data = p.plannedenddate.ToString();
                        p.plannedenddate = Convert.ToDateTime(value);
                        break;
                    case "deadline_date":
                        old_data = p.deadline_date.ToString();
                        p.deadline_date = Convert.ToDateTime(value);
                        break;
                    case "cost":
                        old_data = p.cost.ToString();
                        p.cost = Convert.ToInt32(value);
                        break;
                    case "institute_id":
                        old_data = p.institute_id.ToString();
                        p.institute_id = Convert.ToInt32(value);
                        break;
                    case "stage_id":
                        old_data = p.project_stage_id.ToString();
                        p.project_stage_id = Convert.ToInt32(value);
                        break;
                }

                pm.SaveChanges();





                #region adding project update to the log
                string log_text = "project field " + field_name + " changed from " + old_data + " to " + value;
                log l = new log()
                {
                    log_date = DateTime.Now,
                    log_text = log_text
                };

                pm.logs.Add(l);
                pm.SaveChanges();

                project_log pl = new project_log
                {
                    log_id = l.log_id,
                    project_id = project_id
                };
                pm.project_log.Add(pl);
                pm.SaveChanges();

                #endregion

                return Json(new { Success = true });
            }
            catch(Exception)
			{
                throw;
			}
		}

        public ActionResult delete_proj_gross(int project_id,int gross_id)
		{
			try
			{
                var g = (from gm in pm.gross_marign
                         where gm.project_id == project_id
                         select gm).FirstOrDefault();

                string log_text = "project with id=" + project_id + " removed Gross Marign with data gross_marign_id=" + g.gross_marign_id.ToString() +
                    " description=" + g.description + " gross_date=" + g.gross_date.ToString() + " quantity=" + g.quantity + " Amount=" + g.Amount.ToString() +
                    " funder=" + g.user.username + " type=" + g.gross_marign_type.gross_marign_typename;

                pm.gross_marign.Remove(g);


                pm.SaveChanges();

                

                log l = new log()
                {
                    log_date = DateTime.Now,
                    log_text = log_text
                };

                pm.logs.Add(l);
                pm.SaveChanges();

                project_log pl = new project_log
                {
                    log_id = l.log_id,
                    project_id = project_id
                };
                pm.project_log.Add(pl);
                pm.SaveChanges();

                return Json(new { Success = true });
            }
            catch(Exception)
			{
                throw;
			}

        }

        public ActionResult delete_proj_attachment(int project_id, int attachment_id)
        {
            try
            {
                var proj_a = (from a in pm.project_attachment
                         where a.project_id == project_id && a.attachment_id == attachment_id
                         select a).FirstOrDefault();

                var delete_file = proj_a.attachemnt;

                string log_text = "project with id=" + project_id + " removed Attachment with data attachment_id=" + proj_a.attachemnt.attachment_id +
                    " attachment_name=" + proj_a.attachemnt.attachment_name + " attachment_path=" + proj_a.attachemnt.attachment_path + " attachment_url=" + proj_a.attachemnt.attachment_url;


                
                pm.project_attachment.Remove(proj_a);
                pm.attachemnts.Remove(delete_file);
               
                pm.SaveChanges();

				//string path = Server.MapPath(proj_a.attachemnt.attachment_path);
				FileInfo file = new FileInfo(proj_a.attachemnt.attachment_path);
				if (file.Exists)//check file exsit or not  
				{
					file.Delete();
				}


				log l = new log()
                {
                    log_date = DateTime.Now,
                    log_text = log_text
                };

                pm.logs.Add(l);
                pm.SaveChanges();

                project_log pl = new project_log
                {
                    log_id = l.log_id,
                    project_id = project_id
                };
                pm.project_log.Add(pl);
                pm.SaveChanges();

                return Json(new { Success = true });
            }
            catch (Exception)
            {
                throw;
            }

        }


        public ActionResult project_main_page_search(string searchText)
		{
            Dictionary<string, projectView> projects = new Dictionary<string, projectView>();
            Dictionary<string,stagesView> stages = new Dictionary<string, stagesView>();


            var stages_query = (from s in pm.project_stage
                                where s.stage_name.Contains(searchText)
                                select new stagesView
                                {
                                    stage_id = s.stage_id,
                                    stage_name = s.stage_name

                                }).ToList();



			var projects_query = (from p in pm.projects
								  where p.projectname.Contains(searchText) || p.cost.ToString() == searchText || p.institute.institutename.Contains(searchText) ||
								  p.user.username.Contains(searchText) || p.status.status_name.Contains(searchText) || p.project_stage.stage_name.Contains(searchText)
								  select new projectView
								  {
									  project_id = p.project_id,
									  projectname = p.projectname,
									  plannedstartdate = p.plannedstartdate,
									  plannedenddate = p.plannedenddate,
									  description = p.description,
									  cost = p.cost,
									  institute_id = p.institute_id,
									  project_manager_id = p.project_manager_id,
									  deadline_date = p.deadline_date,
									  project_stage_id = p.project_stage_id,
									  client = p.client,
									  project_status = p.project_status,

									  stage_name = p.project_stage.stage_name,
									  institute_name = p.institute.institutename,
									  status_name = p.status.status_name,
									  client_name = p.user.username
								  }).ToList();



            if (stages_query != null && projects_query == null)
			{

                 projects_query = (from p in pm.projects
                                      where p.project_stage.stage_name.Contains(searchText)
                                      select new projectView
                                      {
                                          project_id = p.project_id,
                                          projectname = p.projectname,
                                          plannedstartdate = p.plannedstartdate,
                                          plannedenddate = p.plannedenddate,
                                          description = p.description,
                                          cost = p.cost,
                                          institute_id = p.institute_id,
                                          project_manager_id = p.project_manager_id,
                                          deadline_date = p.deadline_date,
                                          project_stage_id = p.project_stage_id,
                                          client = p.client,
                                          project_status = p.project_status,

                                          stage_name = p.project_stage.stage_name,
                                          institute_name = p.institute.institutename,
                                          status_name = p.status.status_name,
                                          client_name = p.user.username
                                      }).ToList();
            }

            if (stages_query == null && projects_query != null)
            {
                stages_query = (from p in projects_query
                                select new stagesView
                                {
                                    stage_id = (int)p.project_stage_id,
                                    stage_name = p.stage_name
                                }).ToList();
            }


            foreach (var p in projects_query)
            {
                projects[p.project_id.ToString()] = p;
            }
            foreach (var s in stages_query)
            {
                stages[s.stage_id.ToString()] = s;
            }

            return Json(new { projects, stages }, JsonRequestBehavior.AllowGet);
		}

        public ActionResult save_project_tags(List<int> tags_ids, int project_id)
		{
			try
			{
				#region adding tags
				var t_list = (from t in pm.project_tag
                              where t.project_id == project_id
                              select t).ToList();
               

                foreach (var t in t_list)
				{
                    pm.project_tag.Remove(t);
                }
                foreach (var t_id in tags_ids)
                {                
                    pm.project_tag.Add(new project_tag { tag_id = t_id, project_id = project_id });
                }
                pm.SaveChanges();
                #endregion


                #region adding logs
                string log_text = "project with id =" + project_id.ToString() + " changed the tags from" + t_list.ToString() + " into " + tags_ids.ToString();
                log l = new log()
                {
                    log_text = log_text,
                    log_date = DateTime.Now
                };
                pm.logs.Add(l);
                pm.project_log.Add(new project_log { log_id = l.log_id, project_id = project_id });
                pm.SaveChanges();
                #endregion

                return Json(new { success="success" }, JsonRequestBehavior.AllowGet);

            }
			catch (Exception)
			{
                throw;
			}
            
		}

        public ActionResult load_more(int skip = 2,int take = 2)
		{
			try
			{
                List<projectView> all_projects;
                Dictionary<string, projectView> projects = new Dictionary<string, projectView>();

                all_projects = (from p in pm.projects
                                orderby p.project_id
                                select new projectView
                                {
                                    project_id = p.project_id,
                                    projectname = p.projectname,
                                    plannedstartdate = p.plannedstartdate,
                                    plannedenddate = p.plannedenddate,
                                    plannedstartdate_string = p.plannedstartdate.ToString(),
                                    plannedenddate_string = p.plannedenddate.ToString(),
                                    description = p.description,
                                    cost = p.cost,
                                    institute_id = p.institute_id,
                                    project_manager_id = p.project_manager_id,
                                    deadline_date = p.deadline_date,
                                    deadline_date_string = p.deadline_date.ToString(),
                                    project_stage_id = p.project_stage_id,
                                    client = p.client,
                                    project_status = p.project_status,

                                    stage_name = p.project_stage.stage_name,
                                    institute_name = p.institute.institutename,
                                    status_name = p.status.status_name,
                                    client_name = p.user.username,

                                    project_tags = (from t in pm.project_tag
                                                    where t.project_id == p.project_id
                                                    select new tagView
                                                    {
                                                        tag_id = t.tag.tag_id,
                                                        tagname = t.tag.tagname

                                                    }).ToList(),

                                    project_update_progress = (from u in pm.project_updates
                                                       where u.project_id == p.project_id
                                                       orderby u.update_date descending
                                                               select new projectUpdatesView
                                                               {
                                                                   update_id = u.update_id,
                                                                   description = u.update_description,
                                                                   update_name = u.update_name,
                                                                   update_date = u.update_date,
                                                                   update_date_string = u.update_date.ToString(),
                                                                   status = u.update_status,
                                                                   status_name = u.status.status_name,
                                                                   progress = u.update_progress,
                                                                   updater = u.update_author_id,
                                                                   updater_name = u.user.username,
                                                                   project_id = u.project_id
                                                               }).ToList()

                                }).Skip(skip).ToList();
                all_projects = all_projects.Take(take).ToList();


				foreach (var p in all_projects)
				{
					projects[p.project_id.ToString()] = p;
				}

				return Json(new { projects }, JsonRequestBehavior.AllowGet);

            }
			catch(Exception)
			{
                throw;
			}
		}

        [HttpPost]
        public ActionResult save_gross_margin(gross_marign gross_obj)
		{
			try
			{
                if(gross_obj != null)
				{
                    pm.gross_marign.Add(gross_obj);
				}

                #region adding logs
                string log_text = "project with id =" + gross_obj.project_id.ToString() + " added new margin with ID " + gross_obj.gross_marign_id.ToString() + " and " + gross_obj.description;
                log l = new log()
                {
                    log_text = log_text,
                    log_date = DateTime.Now
                };
                pm.logs.Add(l);
                pm.project_log.Add(new project_log { log_id = l.log_id, project_id = gross_obj.project_id });
                pm.SaveChanges();
                #endregion


                return Json(gross_obj, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
			{
                throw;
			}
		}

        [HttpGet]
        public ActionResult project_total_margin(int id)
		{
			try
			{
                var margins = (from m in pm.gross_marign
                               where m.project_id == id
                               select m).ToList();
                int size = margins.Count();
                double? total = 0;
                for(int i=0;i<size;i++)
				{
                    total = total + (margins[i].Amount * margins[i].quantity);
				}

                return Json(total, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
			{
                throw;
			}
		}

        public ActionResult save_progress(int project_id, float update_progress, string update_name, int status_id, string status_name)
		{
			try
			{
                project_updates pu = new project_updates()
                {
                    project_id = project_id,
                    update_name = update_name,
                    update_date = DateTime.Now,
                    update_status = status_id,
                    update_progress = update_progress,
                    //take care this is manually entered!
                    update_author_id = 6

                };

                pm.project_updates.Add(pu);

                string log_text = "Project with id = " + project_id.ToString() + "updated to progress "+ update_progress.ToString() + " and its name is "+ update_name+" and with status id "+ status_id.ToString();
                DateTime now = DateTime.Now;
                log l = new log()
                {
                    log_date = DateTime.Now,
                    log_text = log_text
                };

                pm.logs.Add(l);
                

                project_log pl = new project_log
                {
                    log_id = l.log_id,
                    project_id = project_id
                };


                pm.project_log.Add(pl);
                pm.SaveChanges();

                projectUpdatesView psv = new projectUpdatesView()
                {
                    update_id = pu.update_id,
                    description = pu.update_description,
                    update_name = pu.update_name,
                    update_date = pu.update_date,
                    update_date_string = pu.update_date.ToString(),
                    status = pu.update_status,
                    status_name = status_name,
                    progress = pu.update_progress,
                    updater = pu.update_author_id,
                    project_id = pu.project_id
                };



                return Json(psv, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
			{
                throw;
			}
		}

        public ActionResult save_new_manager(UserView project_manager)
		{
			try
			{
                user u = new user()
                {
                    username = project_manager.username,
                    pass = project_manager.pass,
                    level_code = project_manager.level_code,
                    institute_id = project_manager.institute_id
                };

                pm.users.Add(u);             
                pm.SaveChanges();



                return Json(project_manager, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
			{
                throw;
			}
		}      

    }
}