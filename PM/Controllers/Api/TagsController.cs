using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PM.Models;

namespace PM.Controllers.Api
{
    public class TagsController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();



       
        [Route("util/tags")]
        public IQueryable<string> Gettags()
        {
            return db.tags.Select(t => t.tagname) ;
        }

        [Route("util/attachments")]
        public IQueryable<string> Getattatchments(int id)
        {
            return db.attachemnts.Select(a => a.attachment_name);
        }

        [Route("util/statuses")]
        public IQueryable<string> Getstatuses(int id)
        {
            return db.status.Select(s => s.status_name);
        }





        [ResponseType(typeof(tag))]
        public IHttpActionResult Gettag(int id)
        {
            tag tag = db.tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag);
        }

        // PUT: api/Tags/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttag(int id, tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tag.tag_id)
            {
                return BadRequest();
            }

            db.Entry(tag).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tagExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        // i will let the tag API handle all the tasks relevant stuff => tag, attachments , status


        // POST: api/Tags
        [ResponseType(typeof(tag))]
        public IHttpActionResult Posttag(tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tags.Add(tag);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tag.tag_id }, tag);
        }

        // DELETE: api/Tags/5
        [ResponseType(typeof(tag))]
        public IHttpActionResult Deletetag(int id)
        {
            tag tag = db.tags.Find(id);
            if (tag == null)
            {
                return NotFound();
            }

            db.tags.Remove(tag);
            db.SaveChanges();

            return Ok(tag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tagExists(int id)
        {
            return db.tags.Count(e => e.tag_id == id) > 0;
        }
    }
}