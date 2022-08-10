using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PM.Models;
using PM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace PM.Hubs
{

    [HubName("task")]
    public class TaskHub : Hub 
    {
      

    

        private project_managementEntities1 db = new project_managementEntities1();



        public override Task OnConnected()
        {
        
      
            
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
           

            return base.OnDisconnected(stopCalled);
        }

        public string server_get_task_details(int id)
        {
            var task = db.tasks.Include(t => t.user).Where(t => t.task_id == id).Select(t => new { t.task_id, t.task_name, t.task_description, task_planned_start = t.task_planned_start, t.task_planned_end , t.user.username , t.status_id}).ToList();

            //todo get ONLY users to this project 
            var users = db.users.Select(u => new { u.username  , u.user_id}).ToList();

            var tags = db.tags.Select(t => new {t.tag_id , t.tagname }).ToList();
            var statuses = db.status.Select(s => new { s.status_id , s.status_name }).ToList();



            //get this task's tags , assignees 
            var task_assignees = db.task_assignedemployee.Include(t => t.user).Where(t => t.task_id == id).Select(t => new { t.user_id, t.user.username });
            var task_tags = db.task_tags.Include(t => t.tag).Where(t => t.task_id == id).Select(t => new { t.tag_id, t.tag.tagname });


            //public task task { get; set; }
            //public List<tag> tags { get; set; }
            //public List<status> state { get; set; }

            //public List<attachemnt> attatchments { get; set; }
            //public List<task_logs> logs { get; set; }
            //public List<task_assignedemployee> assignees { get; set; }



            var output = JsonConvert.SerializeObject(new { users , task, tags ,statuses , task_assignees , task_tags });

            return output;

        }
            public string server_get_tasks()
        {

            

            //var tasks = db.tasks.Select(t=> new { t.task_name , t.task_description , task_planned_start = t.task_planned_start.Value.ToString("dd/MM/yyyy"), task_planned_end = t.task_planned_end.Value.ToString("dd/MM/yyyy") }).ToList();
            var tasks = db.tasks.Select(t => new { t.task_id , t.sub_task, t.task_name, t.task_description, task_planned_start = t.task_planned_start ,  t.task_planned_end , t.status_id , t.task_supervisor}).ToList();

            var statuses = db.status.ToList();
            var output = JsonConvert.SerializeObject(new { tasks , statuses }); 

            return output ; 

            //Clients.All.client_get_tasks(tasks);


            //Another syntax  
            //Clients.All.Send("clientMethod", msg);
        }





        // senario#1 :: client sends data to server to process and then respond back

        //edy el 3eesh l5bazo :) 
        public void server_get_task(string task_id , string task_name = null , string task_deadline = null , string task_desc = null , string task_planned_start = null, string task_planned_end = null , int task_supervisor = 0, bool error= false)
        {
            int task_id_int = int.Parse(task_id);

            var task = db.tasks.FirstOrDefault(t => t.task_name == task_name);
            DateTime? start = null;
            DateTime? end = null;
            DateTime? dead_line = null;

            //edit sub_task case
            if (task != null)
                {

                try
                {
                    dead_line = DateTime.Parse(task_deadline);

                }
                catch (Exception e)
                {

                }


                task.task_deadline = dead_line;
                db.SaveChanges();

                Clients.All.client_get_task(null, false, null, true, true); // if this line is executed after the next >> an exception will stem hmm :)



                create_task_log(task_id_int, "Sub Task is Edited");

                return;
                }


    

            try
            {
                 start = DateTime.Parse(task_planned_start);
                
            }
            catch (Exception e )
            {
              
            }
            try
            {
                end = DateTime.Parse(task_planned_end);

            }
            catch (Exception e)
            {

            }
            try
            {
                dead_line = DateTime.Parse(task_deadline);

            }
            catch (Exception e)
            {
                throw e; 
            }




            var new_task = new task { task_name = task_name, task_description = task_desc, task_planned_start = start, task_planned_end = end, task_deadline = dead_line  };
            db.tasks.Add(new_task);
            db.SaveChanges();

            //update the old task's sub_task id 
        
            db.tasks.Where(t => t.task_id == task_id_int).ToList().ForEach(t => t.sub_task = new_task.task_id);
              db.SaveChanges();



            //(task, error = false, errors = null, edited = false, sub_task = null)
            Clients.All.client_get_task(new_task, false , null , false , true); // if this line is executed after the next >> an exception will stem hmm :)


            
            create_task_log(new_task.task_id, "Sub Task is created");

            

           

            //Another syntax  
            //Clients.All.Send("clientMethod", msg);
        }

        // senario#1 END 

        public void create_task_log(int? task_id , string log_text)
        {
            var log = new log { log_text = log_text, log_date = DateTime.UtcNow.Date };
            db.logs.Add(log);
            db.SaveChanges();
            db.task_logs.Add(new task_logs { task_id = (int)task_id, log_id = log.log_id });
            db.SaveChanges();
            //return log_text ;
        }

        public List<string> get_task_logs(int task_id)
        {
            return db.task_logs.Include(t => t.log).Where(t => t.task_id == task_id).Select(t => t.log.log_text).ToList();
        }


        // MUST : I HAVE TO PASS THE CURRENT SELECTED ITEMS FROM UI AS IS :) otherwise what is not in selected will get deleted 
        public int update_task_details(string updates , string sub_task_name =null , string sub_task_task_deadline =null , bool add_sub_task_cmd = false)
        {
            var update_s = (new JavaScriptSerializer()).Deserialize<TaskUpdates>(updates);

            // first: if i was adding/editing a subtask :-

      
            var sub_task = db.tasks.FirstOrDefault(t => t.task_id == update_s.sub_task_id);
            DateTime? start = null;
            DateTime? end = null;
            DateTime? dead_line = null;
            try
            {
                dead_line = DateTime.Parse(sub_task_task_deadline);

            }
            catch (Exception e)
            {

            }

            //edit sub_task case
            if (sub_task != null)
            {


                //todo sub_task_assignees :(
                sub_task.task_name = sub_task_name;
                sub_task.task_deadline = dead_line;
                //sub_task.status_id = update_s.status;
                db.SaveChanges();

                Clients.All.client_get_task(new { sub_task.task_id , sub_task_name , dead_line }, false, null, true, true); // if this line is executed after the next >> an exception will stem hmm :)



                create_task_log(update_s.sub_task_id, "Sub Task is Edited");

                return (int) update_s.sub_task_id; 
            }
            else if(add_sub_task_cmd)
            {


                var new_task = new task { task_name = sub_task_name, task_deadline = dead_line , status_id = update_s.status };
                db.tasks.Add(new_task);
                db.SaveChanges();

                //update the old task's sub_task id 

                db.tasks.Where(t => t.task_id == update_s.task_id).ToList().ForEach(t => t.sub_task = new_task.task_id);
                db.SaveChanges();



                //(task, error = false, errors = null, edited = false, sub_task = null)
                Clients.All.client_get_task(new_task, false, null, false, true); // if this line is executed after the next >> an exception will stem hmm :)


                create_task_log(new_task.task_id, "Sub Task is created");

                return new_task.task_id ;

            }



            // a small addon till i improve this method algorithm 
            task task = null;
            if (update_s.status != null)
            {
                task = db.tasks.Find(update_s.task_id);
                task.status_id = update_s.status;
                db.SaveChanges();
            }

       

            // i will get the data first from db > if they found >> update , not >> add


       

            //update_s.tags.ForEach(tag_id => db.task_tags.AddOrUpdate(t => t.id, new task_tags { task_id = update_s.task_id, tag_id = tag_id }) );

            //todo workout this sub_task query once i handle UI of selects :(
            //db.task_assignedemployee.AddOrUpdate(t => new { t.user_id , t.task_id}, update_s.assignees_new.Select( a=>  new task_assignedemployee { task_id = (int) update_s.sub_task_id, user_id = a }).ToArray() );


            db.task_tags.AddOrUpdate(t => new { t.tag_id, t.task_id }, update_s.tags.Select(a => new task_tags { task_id = (int)update_s.task_id, tag_id = a }).ToArray());

            db.task_assignedemployee.AddOrUpdate(t => new { t.user_id, t.task_id }, update_s.assignees.Select(a => new task_assignedemployee { task_id = (int) update_s.task_id, user_id = a }).ToArray());

            //delete what is not in the selected items :(

            var items = db.task_tags.Where(t => !update_s.tags.Contains(t.tag_id));
            db.task_tags.RemoveRange(items);

            var items2 = db.task_assignedemployee.Where(t => !update_s.assignees.Contains(t.user_id));
            db.task_assignedemployee.RemoveRange(items2);

            //todo workout this sub_task query once i handle UI of selects :(
            //var items3 = db.task_assignedemployee.Where(t => !update_s.assignees_new.Contains(t.user_id));
            //db.task_assignedemployee.RemoveRange(items3);


            //db.task_tags.AddRange(update_s.tags.Select(tag_id => new task_tags { task_id = update_s.task_id, tag_id = tag_id }));

            //db.task_assignedemployee.AddRange(update_s.assignees.Select(assignee_id => new task_assignedemployee { task_id = update_s.task_id, user_id = assignee_id }));
            db.SaveChanges();
            create_task_log(update_s.task_id, "Task updated");
            Clients.All.client_get_task(task != null ? new { task.task_id , task.task_name , task.status_id} : null , false , null , true); // if this line is executed after the next >> an exception will stem hmm :)



            //Clients.All.task_edited();

            // updates task's : tags , assignees , statuses<todo>
            return sub_task != null ? sub_task.task_id :  0 ;
        }


        public string serverMethod2()
        {
            return "data from the server"; 
        }


        void recurse_delete(int? id)
        {
            task task = db.tasks.Find(id);

            if (task != null)
            {

                db.task_logs.RemoveRange(db.task_logs.Where(l => l.task_id == id));


                db.task_tags.RemoveRange(db.task_tags.Where(l => l.task_id == id));


                db.task_assignedemployee.RemoveRange(db.task_assignedemployee.Where(l => l.task_id == id));

                //this call doesn't trigger the physical deletion from disk 
                db.task_attachments.RemoveRange(db.task_attachments.Where(l => l.task_id == id));

                db.SaveChanges();

                db.tasks.Remove(task);
                recurse_delete(task.sub_task);
            }
            else
            {
                db.SaveChanges();
            }
          

        }

        public string server_delete_task( int taskId , int sub_task_id , bool delete_sub_task)
        {
            // task has : tags , logs , attachments ,assigned employees . to be deleted

            if (delete_sub_task)
            {

                db.task_logs.RemoveRange(db.task_logs.Where(l => l.task_id == sub_task_id));


                db.task_tags.RemoveRange(db.task_tags.Where(l => l.task_id == sub_task_id));


                db.task_assignedemployee.RemoveRange(db.task_assignedemployee.Where(l => l.task_id == sub_task_id));

                //this call doesn't trigger the physical deletion from disk 
                db.task_attachments.RemoveRange(db.task_attachments.Where(l => l.task_id == sub_task_id));

                db.SaveChanges();


                // 2 cases here :- if i'm deleting from a great-parent task OR if i'm deleting from a direct parent task
                // the easy case : case#2 :-


                var parent_task = db.tasks.Find(taskId);
                if(parent_task.sub_task == sub_task_id)
                {
                    //if it has a parent null the reference on the parent, if not >> leave it as is 
                    if (parent_task != null)
                    {
                        parent_task.sub_task = null;
                        recurse_delete(sub_task_id);

                    }

                }else // the great parent case >> the more difficult case
                {
                    //2 handling mechanics 
                    // either i don't allow deleting from a great parent task to begin with
                    // or i find the direct parent and detach the link with its sub-task
                    // approach 1 makes more sense as the deleting responsibility is not of the great parent but the direct parent



                }
             
                db.SaveChanges();

            }
            else
            {

                db.task_logs.RemoveRange(db.task_logs.Where(l => l.task_id == taskId));


                db.task_tags.RemoveRange(db.task_tags.Where(l => l.task_id == taskId));


                db.task_assignedemployee.RemoveRange(db.task_assignedemployee.Where(l => l.task_id == taskId));

                //this call doesn't trigger the physical deletion from disk 
                db.task_attachments.RemoveRange(db.task_attachments.Where(l => l.task_id == taskId));

                db.SaveChanges();


                recurse_delete(taskId);

                //task task = db.tasks.Find(taskId);

                //if (task != null)
                //{
                //    db.tasks.Remove(task);
                //}

                //db.SaveChanges();


            }




            return "done"; 
        }





    }
}