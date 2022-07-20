using Microsoft.EntityFrameworkCore;
using PM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace PM.Controllers.Api
{
    public class InstitutesTypesController : ApiController
    {
        private project_managementEntities1 _context;

        public InstitutesTypesController()
        {
            _context = new project_managementEntities1();
        }


        // GET api/<controller>
        public IHttpActionResult GetInstitutesTypes()
        {

            // LOL THE TARGET PROPERTY NOT THE NAVIGATING PROPERTY 
            var institutesTypesQuery = _context.institute_type.Select(i => new { i.type_id, i.typename });


            return Ok(institutesTypesQuery);

        }

        // GET api/<controller>/5
        public IHttpActionResult GetInstitutesType(int id)
        {

            var institute_type_indb = _context.institute_type.SingleOrDefault(c => c.type_id == id);

            if (institute_type_indb == null)
                return NotFound();

            return Ok(institute_type_indb);
        }

        // POST api/<controller>
        public IHttpActionResult PostInstitutesType(institute_type type)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            _context.institute_type.Add(type);
            _context.SaveChanges();



            return Created(new Uri(Request.RequestUri + "/" + type.type_id), type);
        }

        // PUT api/<controller>/5
        public IHttpActionResult PutInstitutesType(int id, [FromBody] string value)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var InstitutesTypeInDb = _context.institute_type.SingleOrDefault(c => c.type_id == id);

            if (InstitutesTypeInDb == null)
                return NotFound();



            _context.SaveChanges();

            return Ok();
        }

        // DELETE api/<controller>/5
        public IHttpActionResult DeleteInstitutesType(int id)
        {

            var InstitutesTypeInDb = _context.institute_type
                .SingleOrDefault(i => i.type_id == id);

            if (InstitutesTypeInDb == null)
                return NotFound();


            _context.institute_type.Remove(InstitutesTypeInDb);
            _context.SaveChanges();

            return Ok();

        }
    }
}