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
    public class attachemntsController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: api/attachemnts
        public IQueryable<attachemnt> Getattachemnts()
        {
            return db.attachemnts;
        }

        // GET: api/attachemnts/5
        [ResponseType(typeof(attachemnt))]
        public IHttpActionResult Getattachemnt(int id)
        {
            //attachemnt attachemnt = db.attachemnts.Find(id);
            var query = (from ids in db.project_attachment
                         where ids.project_id == id
                         select ids.attachemnt.attachment_name).ToList();


            if (query == null)
            {
                return NotFound();
            }

            return Ok(query);
        }

        // PUT: api/attachemnts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putattachemnt(int id, attachemnt attachemnt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attachemnt.attachment_id)
            {
                return BadRequest();
            }

            db.Entry(attachemnt).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!attachemntExists(id))
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

        // POST: api/attachemnts
        [ResponseType(typeof(attachemnt))]
        public IHttpActionResult Postattachemnt(attachemnt attachemnt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.attachemnts.Add(attachemnt);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = attachemnt.attachment_id }, attachemnt);
        }

        // DELETE: api/attachemnts/5
        public IHttpActionResult Deleteattachemnt(int id)
        {
            attachemnt attachemnt = db.attachemnts.Find(id);
            var pa_query = (from pa in db.project_attachment
                        where pa.attachment_id == attachemnt.attachment_id
                        select pa).FirstOrDefault();

            if (pa_query == null || attachemnt == null)
            {
                return NotFound();
            }

            db.project_attachment.Remove(pa_query);
            db.attachemnts.Remove(attachemnt);
            db.SaveChanges();

            return Ok(attachemnt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool attachemntExists(int id)
        {
            return db.attachemnts.Count(e => e.attachment_id == id) > 0;
        }
    }
}