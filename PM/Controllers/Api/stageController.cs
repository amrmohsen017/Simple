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
using Newtonsoft.Json;
using PM.Models;
using PM.ViewModels;

namespace PM.Controllers.Api
{
    public class stageController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: api/stage
        public HttpResponseMessage Get_stage()
        {
            //var p = db.projects.ToList();
            var s = db.project_stage.ToList();
            
            //projectStage ps = new projectStage() {projects = p, stages = s };
            //var json = JsonConvert.SerializeObject(ps);
            return Request.CreateResponse(HttpStatusCode.OK, s);
        }


       







        //public HttpResponseMessage Getproject()
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, db.projects.ToList());
        //}







        // GET: api/stage/5
        //[ResponseType(typeof(project))]
        //public IHttpActionResult Getproject(int? id)
        //{
        //    project project = db.projects.Find(id);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(project);
        //}


        // PUT: api/stage/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putproject_stage(int id, project_stage project_stage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project_stage.stage_id)
            {
                return BadRequest();
            }

            db.Entry(project_stage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!project_stageExists(id))
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

        

        // DELETE: api/stage/5
        [ResponseType(typeof(project_stage))]
        public IHttpActionResult Deleteproject_stage(int id)
        {
            project_stage project_stage = db.project_stage.Find(id);
            if (project_stage == null)
            {
                return NotFound();
            }

            db.project_stage.Remove(project_stage);
            db.SaveChanges();

            return Ok(project_stage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool project_stageExists(int id)
        {
            return db.project_stage.Count(e => e.stage_id == id) > 0;
        }
    }
}