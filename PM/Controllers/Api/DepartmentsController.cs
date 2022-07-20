using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using PM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PM.Controllers.Api
{
    public class DepartmentsController : ApiController
    {
        private project_managementEntities1 _context;

        public DepartmentsController()
        {
            _context = new project_managementEntities1();
        }


        // GET api/<controller>
        public IHttpActionResult GetDepartments()
        {

            // LOL THE TARGET PROPERTY NOT THE NAVIGATING PROPERTY 
            var departmentsQuery = _context.departments.Select(i => new { i.departementid, i.departmentname });



            return Ok(departmentsQuery);

        }

        // GET api/<controller>/5
        public IHttpActionResult GetDepartment(int id)
        {

            var user = _context.departments.SingleOrDefault(c => c.departementid == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST api/<controller>
        public IHttpActionResult PostDepartment(department departme)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            _context.departments.Add(departme);
            _context.SaveChanges();



            return Created(new Uri(Request.RequestUri + "/" + departme.departementid), departme);
        }

        // PUT api/<controller>/5
        public IHttpActionResult PutDepartment(int id, [FromBody] string value)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var departmentInDb = _context.departments.SingleOrDefault(c => c.departementid == id);

            if (departmentInDb == null)
                return NotFound();



            _context.SaveChanges();

            return Ok();
        }

        // DELETE api/<controller>/5
        public IHttpActionResult DeleteDepartment(int id)
        {




            //constraints again
            var referred_institutes = _context.institutes.Where(i => i.department_id == id);

            referred_institutes.ForEach(i => i.department_id = null);

            //_context.Entry(institute).Property("department_id").IsModified = true; // didn't work idk why :(
            // had to do a little trick here to bypass my custom validations >> further reading on EF context tracking...
            _context.SaveChanges();

            var departmentInDb = _context.departments
               .SingleOrDefault(i => i.departementid == id);

            if (departmentInDb == null)
                return NotFound();



            _context.departments.Remove(departmentInDb);
            _context.SaveChanges();

            return Ok();

        }
    }
}