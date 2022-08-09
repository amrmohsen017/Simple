using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PM.Hubs;
using PM.Models;



namespace PM.Controllers.Api
{
    public class tasksController : ApiController
    {
        private project_managementEntities1 db = new project_managementEntities1();


    

        // GET: api/tasks/5
        [ResponseType(typeof(task))]
        public IHttpActionResult Gettask(int id)
        {
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/tasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttask(int id, task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.task_id)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!taskExists(id))
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



        [ResponseType(typeof(task))]
        public IHttpActionResult Posttask(task task)
        {

            var hub = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();



            if (!ModelState.IsValid)
            {

                var errors = ModelState.Keys
                    .SelectMany(key => ModelState[key].Errors.Select(x =>  x.ErrorMessage))
                    .ToList();



                hub.Clients.All.client_get_task(null, true , errors);
                return BadRequest(ModelState);
            }


            db.tasks.Add(task);


            db.SaveChanges();

            hub.Clients.All.client_get_task(task, false , null );

            // logs logic :) 
            var log = new log { log_text = "Task was created" , log_date = DateTime.UtcNow.Date };
            db.logs.Add(log);
            db.SaveChanges();
            db.task_logs.Add(new task_logs { task_id = task.task_id, log_id = log.log_id });
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.task_id  }, task);
        }




        void find_all_subtasks(int initial_task_id , List<int> subtasks )
        {
            var task = db.tasks.Find(initial_task_id);


            List<int?> sub_task_ids = db.tasks.Where(t => t.task_id == initial_task_id).Where(t => t.sub_task != null).Select(t => t.sub_task).ToList();
            //int count_no_filter = sub_task_ids.Count() - 1; // minus the parent task record <in case of the assumed criterion is met which is not confirmed to me thus far>:) 

            task temp_task = null;
            //invalidate sub_task_ids that point to nothing :)
            sub_task_ids.ToList().ForEach(sub_task_id =>
            {
                temp_task = db.tasks.Find(sub_task_id);
                if (temp_task == null)
                {
                    task.sub_task = null;
                    sub_task_ids.Remove(sub_task_id);
                    db.SaveChanges();
                }
                else
                {
                    subtasks.Add((int)sub_task_id);
                }


            });
            if (sub_task_ids.Count() == 0) return; 
            find_all_subtasks(subtasks.Last(), subtasks);



        }
        // POST: api/tasks
        [HttpPost]
        [Route("test")]
        public IHttpActionResult post_sub_task(FormDataCollection form)
        {




            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            string draw = form.Get("draw");
            string start = form.Get("start");
            string rowperpage = form.Get("length");                     // Rows display per page
            string columnIndex = form.Get("order[0][column]");    // Column index
            string columnName = form.Get($"columns[{columnIndex}][data]");    // Column name
            string columnSortOrder = form.Get("order[0][dir]");  // asc or desc
            string searchValue = form.Get("search[value]");          // Search value


            string task_id = form.Get("task_id");
            if(task_id == "")
            {
                return Ok(new
                {

                    draw = int.Parse(draw),
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = "nothing"
                });
            }
            // line that errorsssssssssssss

            // this is the parent task_id forreal :)
            int? task_id_int = int.Parse(task_id) ;








            //int count_no_filter = db.tasks.Where(t => t.task_id == int.Parse(task_id)).Count();

            // subtasks ALGOs
            // task_id is exepcted to duplicate per each sub_task ?? no 

            List<int> sub_task_ids = new List<int>();
            find_all_subtasks((int) task_id_int, sub_task_ids);




            int count_no_filter = sub_task_ids.Count() ; // minus the parent task record :) 

            int count_filter = db.tasks.Where(t=> sub_task_ids.Contains(t.task_id))
                .Count(t => t.task_name.Contains(searchValue) ||

            t.task_description.Contains(searchValue)

            );


            var sub_tasks = db.tasks.Where(t => sub_task_ids.Contains(t.task_id)).Where(t => t.task_name.Contains(searchValue) ||

               t.task_description.Contains(searchValue)

                );



            Dictionary<string, Func<task, object>> field_mapper = new Dictionary<string, Func<task, object>>()
                    {
                        {"task_name", t => t.task_name},
                          {"task_description", t => t.task_description},
                            {"task_planned_start", t => t.task_planned_start},
                             {"task_planned_end", t => t.task_planned_end},
                              {"task_deadline", t => t.task_deadline},
                    };


            dynamic output; 

            if (columnSortOrder == "asc")
            {
            

               output = sub_tasks.OrderBy(field_mapper[columnName]) // hack of the day :)
                    .Skip(int.Parse(start)).Take(int.Parse(rowperpage))
                    .Select(t =>  new { t.task_id, t.task_name, t.task_description, t.task_planned_start, t.task_planned_end, t.task_deadline })
                     .ToList()
                ;

            }
            else
            {
                 output = sub_tasks.OrderByDescending(field_mapper[columnName])
                    .Skip(int.Parse(start)).Take(int.Parse(rowperpage))
                    .Select(t => new { t.task_id, t.task_name, t.task_description , t.task_planned_start, t.task_planned_end , t.task_deadline})
                    .ToList()
                ;
            }



            //var final = JsonConvert.SerializeObject(new
            //{

            //    draw = int.Parse(draw),
            //    recordsTotal = count_no_filter,
            //    recordsFiltered = count_filter,
            //    data = output
            //});

            return Ok(new
            {

                draw = int.Parse(draw),
                recordsTotal = count_no_filter,
                recordsFiltered = count_filter,
                data = output
            });



        }



        [HttpPost]
        [Route("tasks/attachments")]
        public IHttpActionResult post_task_attachments(FormDataCollection form)
        {




            string draw = form.Get("draw");
            string start = form.Get("start");
            string rowperpage = form.Get("length");                     // Rows display per page
            string columnIndex = form.Get("order[0][column]");    // Column index
            string columnName = form.Get($"columns[{columnIndex}][data]");//[columnIndex]['data'];    // Column name
            string columnSortOrder = form.Get("order[0][dir]");   // asc or desc
            string searchValue = form.Get("search[value]");          // Search value


            string task_id = form.Get("task_id");
            if (task_id == "")
            {
                return Ok(new
                {

                    draw = int.Parse(draw),
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = "nothing"
                });
            }
            // line that errorsssssssssssss
            int? task_id_int = int.Parse(task_id);








            //int count_no_filter = db.tasks.Where(t => t.task_id == int.Parse(task_id)).Count();

            // subtasks ALGOs
            // task_id is exepcted to duplicate per each sub_task ?? no 

        


            int count_no_filter = db.task_attachments.Where(t=>t.task_id == task_id_int).Count(); // minus the parent task record :) 

            int count_filter = db.task_attachments.Where(t => t.task_id == task_id_int).Include(t=>t.attachemnt)
                .Count(t => t.attachemnt.attachment_name.Contains(searchValue) 

            );




            var attachments = db.task_attachments.Include(t => t.attachemnt).Where(t => 
            
            t.task_id == task_id_int &&
            t.attachemnt.attachment_name.Contains(searchValue) 

                );



            Dictionary<string, Func<task_attachments, object>> field_mapper = new Dictionary<string, Func<task_attachments, object>>()
                    {
                        {"attachment_name", t=> t.attachemnt.attachment_name},
                     
                    };


            dynamic output;

            if (columnSortOrder == "asc")
            {


                output = attachments.OrderBy(field_mapper[columnName]) // hack of the day :)
                     .Skip(int.Parse(start)).Take(int.Parse(rowperpage))
                     .Select(t => new { t.task_id, t.attachmnet_id, t.attachemnt.attachment_name  , t.attachemnt.attachment_url })
                      .ToList()
                 ;

            }
            else
            {
                output = attachments.OrderByDescending(field_mapper[columnName])
                   .Skip(int.Parse(start)).Take(int.Parse(rowperpage))
                   .Select(t => new { t.task_id, t.attachmnet_id, t.attachemnt.attachment_name, t.attachemnt.attachment_url })
                   .ToList()
               ;
            }



            //var final = JsonConvert.SerializeObject(new
            //{

            //    draw = int.Parse(draw),
            //    recordsTotal = count_no_filter,
            //    recordsFiltered = count_filter,
            //    data = output
            //});

            return Ok(new
            {

                draw = int.Parse(draw),
                recordsTotal = count_no_filter,
                recordsFiltered = count_filter,
                data = output
            });



        }

        // DELETE: api/tasks/5
        [Route("tasks/attachments/delete/{id}/{task_id}")]
        public IHttpActionResult DeleteAttachment(int id , int task_id)
        {

            var task_attachments = db.task_attachments.Where(x => x.task_id == task_id && x.attachmnet_id == id ).ToList();



            db.task_attachments.RemoveRange(task_attachments);


            var attachment_record = db.attachemnts.Find(id);
            if (attachment_record == null)
            {
                return NotFound();
            }

            db.attachemnts.Remove(attachment_record);

            try
            {
                // Check if file exists with its full path    
                if (File.Exists(attachment_record.attachment_path))
                {
                    // If file found, delete it    
                    File.Delete(attachment_record.attachment_path);
                  
                }
              
            }
            catch (IOException ioExp)
            {
                throw ioExp;
            }

            db.SaveChanges();
           

            return Ok(attachment_record);
        }














        // DELETE: api/tasks/5
        [ResponseType(typeof(task))]
        public IHttpActionResult Deletetask(int id)
        {
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            db.tasks.Remove(task);
            db.SaveChanges();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool taskExists(int id)
        {
            return db.tasks.Count(e => e.task_id == id) > 0;
        }
    }
}