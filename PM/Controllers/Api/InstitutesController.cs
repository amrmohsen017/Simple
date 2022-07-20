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
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;

namespace PM.Controllers.Api
{


    public class InstitutesController : ApiController
    {
        private project_managementEntities1 _context;

        public InstitutesController()
        {
            _context = new project_managementEntities1();
        }


        //public IHttpActionResult GetCities(int id)
        //{


        //    var institutesQuery = _context.cities.Where( i => i.governmentcode == id)
        //        .Select(i => new {  i.cityname });


        //    return Ok(institutesQuery);

        //}
        //[System.Web.Http.Route("api/station")]
        //public IHttpActionResult GetStation(int id)
        //{


        //    var institutesQuery = _context.stations.Where(i => i.citycode == id)
        //        .Select(i => new { i.stationname });


        //    return Ok(institutesQuery);

        //}

        // GET api/<controller>
        public IHttpActionResult GetInstitutes()
        {

            // LOL THE TARGET PROPERTY NOT THE NAVIGATING PROPERTY 
            var institutesQuery = _context.institutes
                .Include(i => i.institute_address).ThenInclude(i => i.station).ThenInclude(i => i.city).ThenInclude(i => i.governmnet)
                .Include(i => i.department).ToList().Select(i => new { i.institute_id, i.institutename, i.institute_fulladdress, i.email, i.telephone, i.department?.departmentname, i.adress_id, i.institute_address?.station?.stationname, i.institute_address?.city?.cityname, i.institute_address?.governmnet?.governmentname });


            return Ok(institutesQuery);

        }

        // GET api/<controller>/5
        public IHttpActionResult GetUser(int id)
        {

            var user = _context.users.SingleOrDefault(c => c.user_id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST api/<controller>
        public IHttpActionResult PostUser(user usr)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            _context.users.Add(usr);
            _context.SaveChanges();



            return Created(new Uri(Request.RequestUri + "/" + usr.user_id), usr);
        }

        // PUT api/<controller>/5
        public IHttpActionResult PutUser(int id, [FromBody] string value)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userInDb = _context.users.SingleOrDefault(c => c.user_id == id);

            if (userInDb == null)
                return NotFound();



            _context.SaveChanges();

            return Ok();
        }



        // DELETE api/<controller>/5
        public IHttpActionResult DeleteInstitute(int id)
        {

            var instituteInDb = _context.institutes.Include(i => i.institute_address)
                .SingleOrDefault(i => i.institute_id == id);

            if (instituteInDb == null)
                return NotFound();


            //cascade deleting of the dependent entities 
            //var usersInDb = _context.users.Where(u => u.institute_id == instituteInDb.adress_id);
            //_context.users.RemoveRange(usersInDb);

            // users is the child => has foreign key to institutes

            var usersInDb = _context.users.Include(u => u.institute).Where(u => u.institute_id == instituteInDb.institute_id);

            var section_instituteInDb = _context.section_institute.Where(u => u.insti_id == instituteInDb.institute_id);


            var projectInDb = _context.projects.Where(u => u.institute_id == instituteInDb.institute_id);

            //TODO :: handle cascade behaviour out of the box instead of 4 SaveChanges() calls

            _context.projects.RemoveRange(projectInDb);
            _context.SaveChanges();

            _context.section_institute.RemoveRange(section_instituteInDb);
            _context.SaveChanges();

            _context.users.RemoveRange(usersInDb);
            _context.SaveChanges();

            _context.institutes.Remove(instituteInDb);
            _context.SaveChanges();

            return Ok();

        }
    }
}