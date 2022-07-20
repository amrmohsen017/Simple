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
    public class UsersController : ApiController
    {
        private project_managementEntities1 _context;

        public UsersController()
        {
            _context = new project_managementEntities1();
        }


        // GET api/<controller>
        public IHttpActionResult GetUsers()
        {

            // LOL THE TARGET PROPERTY NOT THE NAVIGATING PROPERTY 
            var usersQuery = _context.users
                .Include(u => u.institute)
                .Include(u => u.job).ToList().Select(u => new { u.user_id, u.username, u.telephone, u.email, u.institute.institutename, u.job.jobname });
            //.Include(u => u.level_code)




            return Ok(usersQuery);

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
        public IHttpActionResult DeleteUser(int id)
        {

            var userInDb = _context.users.SingleOrDefault(c => c.user_id == id);

            if (userInDb == null)
                return NotFound();

            _context.users.Remove(userInDb);
            _context.SaveChanges();

            return Ok();

        }
    }
}