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
    public class citiesController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: api/cities
        public IQueryable<city> Getcities()
        {
            return db.cities;
        }

        // GET: api/cities/5
        [ResponseType(typeof(city))]
        public IHttpActionResult Getcity(int id)
        {
            //city city = db.cities.Find(id);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //return Ok(city.cityname);

            var institutesQuery = db.cities.Where(i => i.governmentcode == id)
                .Select(i => new { i.cityname });
            if (institutesQuery == null)
            {
                return NotFound();
            }


            return Ok(institutesQuery);
        }

        // PUT: api/cities/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcity(int id, city city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != city.citycode)
            {
                return BadRequest();
            }

            db.Entry(city).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cityExists(id))
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

        // POST: api/cities
        [ResponseType(typeof(city))]
        public IHttpActionResult Postcity(city city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.cities.Add(city);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = city.citycode }, city);
        }

        // DELETE: api/cities/5
        [ResponseType(typeof(city))]
        public IHttpActionResult Deletecity(int id)
        {
            city city = db.cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }

            db.cities.Remove(city);
            db.SaveChanges();

            return Ok(city);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool cityExists(int id)
        {
            return db.cities.Count(e => e.citycode == id) > 0;
        }
    }
}