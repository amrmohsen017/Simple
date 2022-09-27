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
    public class gross_marignController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: api/gross_marign
        public IQueryable<gross_marign> Getgross_marign()
        {
            return db.gross_marign;
        }

        // GET: api/gross_marign/5
        [ResponseType(typeof(gross_marign))]
        public IHttpActionResult Getgross_marign(int id)
        {
            gross_marign gross_marign = db.gross_marign.Find(id);
            if (gross_marign == null)
            {
                return NotFound();
            }

            return Ok(gross_marign);
        }

        // PUT: api/gross_marign/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putgross_marign(int id, gross_marign gross_marign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gross_marign.gross_marign_id)
            {
                return BadRequest();
            }

            db.Entry(gross_marign).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!gross_marignExists(id))
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

        // POST: api/gross_marign
        [ResponseType(typeof(gross_marign))]
        public IHttpActionResult Postgross_marign(gross_marign gross_marign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.gross_marign.Add(gross_marign);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gross_marign.gross_marign_id }, gross_marign);
        }

        // DELETE: api/gross_marign/5
        [ResponseType(typeof(gross_marign))]
        public IHttpActionResult Deletegross_marign(int id)
        {
            gross_marign gross_marign = db.gross_marign.Find(id);
            if (gross_marign == null)
            {
                return NotFound();
            }

            db.gross_marign.Remove(gross_marign);
            db.SaveChanges();

            return Ok(gross_marign);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool gross_marignExists(int id)
        {
            return db.gross_marign.Count(e => e.gross_marign_id == id) > 0;
        }
    }
}