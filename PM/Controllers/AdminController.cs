using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PM.Models;
using PagedList;
using System.Data.Entity.Validation;

namespace PM.Controllers
{
    public class AdminController : Controller
    {
        project_managementEntities pm = new project_managementEntities();

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
                    updatedP.startdate = p.startdate;
                    updatedP.enddatte = p.enddatte;
                    updatedP.cost = updatedP.cost;
                    updatedP.details = p.details;
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

    }

}