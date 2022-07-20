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
    public class stationsController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: api/stations
        public IQueryable<station> Getstations()
        {
            return db.stations;
        }

        // GET: api/stations/5
        [ResponseType(typeof(station))]
        public IHttpActionResult Getstation(int id)
        {
            //station station = db.stations.Find(id);
            //if (station == null)
            //{
            //    return NotFound();
            //}

            //return Ok(station.stationname);

            var institutesquery = db.stations.Where(i => i.citycode == id)
                .Select(i => new { i.stationname });
            if (institutesquery == null)
            {
                return NotFound();
            }

            return Ok(institutesquery);


        }

        // PUT: api/stations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putstation(int id, station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != station.stationcode)
            {
                return BadRequest();
            }

            db.Entry(station).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!stationExists(id))
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

        // POST: api/stations
        [ResponseType(typeof(station))]
        public IHttpActionResult Poststation(station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.stations.Add(station);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = station.stationcode }, station);
        }

        // DELETE: api/stations/5
        [ResponseType(typeof(station))]
        public IHttpActionResult Deletestation(int id)
        {
            station station = db.stations.Find(id);
            if (station == null)
            {
                return NotFound();
            }

            db.stations.Remove(station);
            db.SaveChanges();

            return Ok(station);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool stationExists(int id)
        {
            return db.stations.Count(e => e.stationcode == id) > 0;
        }
    }
}