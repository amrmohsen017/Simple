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
using PM.ViewModels;
using System.IO;


namespace PM.Controllers.Api
{
    public class projectsController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();

        // GET: api/projects
        public HttpResponseMessage Getprojects()
        {
            var p = db.projects.Select(pr => new { pr.project_id  , pr.projectname ,pr.project_stage_id }).ToList();
            
            return Request.CreateResponse(HttpStatusCode.OK, p);
        }

        // GET: api/projects/5
        [ResponseType(typeof(project))]
        public IHttpActionResult Getproject(int id)
        {
            project project = db.projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }


        // POST: api/stage
        //creating a new project
        //[ResponseType(typeof(project_stage))]
   //     public HttpResponseMessage Postproject(project p)
   //     {
            
   //         //if (p == null)
   //         //{
   //         //	return Request.CreateErrorResponse("the project was sent empty!");
   //         //}
            




			//return Request.CreateResponse(HttpStatusCode.Created, p);
   //     }









        // PUT: api/projects/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putproject(int id, project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.project_id)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!projectExists(id))
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

        

        // DELETE: api/projects/5
        [ResponseType(typeof(project))]
        public IHttpActionResult Deleteproject(int id)
        {
            project project = db.projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            db.projects.Remove(project);
            db.SaveChanges();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool projectExists(int id)
        {
            return db.projects.Count(e => e.project_id == id) > 0;
        }
    }
}