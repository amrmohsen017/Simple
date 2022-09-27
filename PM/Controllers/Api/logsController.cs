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
    public class logsController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: api/logs
        public IQueryable<log> Getlogs()
        {
            return db.logs;
        }

        // GET: api/logs/5
        [ResponseType(typeof(log))]
        public IHttpActionResult Getlog(int id)
        {
            log log = db.logs.Find(id);
            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
        }

        // PUT: api/logs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlog(int id, log log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != log.log_id)
            {
                return BadRequest();
            }

            db.Entry(log).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!logExists(id))
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

        // POST: api/logs
        [ResponseType(typeof(log))]
        public IHttpActionResult Postlog(log log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.logs.Add(log);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = log.log_id }, log);
        }

        // DELETE: api/logs/5
        [ResponseType(typeof(log))]
        public IHttpActionResult Deletelog(int id)
        {
            log log = db.logs.Find(id);
            if (log == null)
            {
                return NotFound();
            }

            db.logs.Remove(log);
            db.SaveChanges();

            return Ok(log);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool logExists(int id)
        {
            return db.logs.Count(e => e.log_id == id) > 0;
        }
    }
}