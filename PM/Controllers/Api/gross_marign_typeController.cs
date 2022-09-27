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
    public class gross_marign_typeController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: api/gross_marign_type
        public HttpResponseMessage Getgross_marign_type()
        {
            var t = db.gross_marign_type.Select(pr => new { pr.id, pr.gross_marign_typename }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, t);
        }

        // GET: api/gross_marign_type/5
        [ResponseType(typeof(gross_marign_type))]
        public IHttpActionResult Getgross_marign_type(int id)
        {
            gross_marign_type gross_marign_type = db.gross_marign_type.Find(id);
            if (gross_marign_type == null)
            {
                return NotFound();
            }

            return Ok(gross_marign_type);
        }

        // PUT: api/gross_marign_type/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putgross_marign_type(int id, gross_marign_type gross_marign_type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gross_marign_type.id)
            {
                return BadRequest();
            }

            db.Entry(gross_marign_type).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!gross_marign_typeExists(id))
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

        // POST: api/gross_marign_type
        [ResponseType(typeof(gross_marign_type))]
        public IHttpActionResult Postgross_marign_type(gross_marign_type gross_marign_type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var query = db.gross_marign_type.Where(x => x.gross_marign_typename == gross_marign_type.gross_marign_typename).FirstOrDefault();
            if(query != null)
			{
                return Ok("duplicated");
			}
            db.gross_marign_type.Add(gross_marign_type);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gross_marign_type.id }, gross_marign_type);
        }

        // DELETE: api/gross_marign_type/5
        [ResponseType(typeof(gross_marign_type))]
        public IHttpActionResult Deletegross_marign_type(int id)
        {
            gross_marign_type gross_marign_type = db.gross_marign_type.Find(id);
            if (gross_marign_type == null)
            {
                return NotFound();
            }

            db.gross_marign_type.Remove(gross_marign_type);
            db.SaveChanges();

            return Ok(gross_marign_type);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool gross_marign_typeExists(int id)
        {
            return db.gross_marign_type.Count(e => e.id == id) > 0;
        }
    }
}