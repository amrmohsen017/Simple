using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PM.Models;

namespace PM.Hubs
{
	public class MyHub : Hub
	{
		public void refreshAnyDatatable(string message)
		{
			Clients.All.addNewMessageToPage(message);
		}

		//will add creation of the project to the log table.
		public void addNewProjectLog(int project_id,string project_name)
		{
			try
			{
				project_managementEntities1 pm = new project_managementEntities1();

				string log_text = "Project with id = " + project_id.ToString() + " and name = " + project_name + " has been created successfully";
				DateTime now = DateTime.Now;
				log new_log = new log
				{
					log_text = log_text,
					log_date = now
				};
				pm.logs.Add(new_log);
				pm.SaveChanges();

				var get_new_log = (from l in pm.logs
								   where l.log_date == now
								   select l).FirstOrDefault();

				project_log pl = new project_log
				{
					log_id = get_new_log.log_id,
					project_id = project_id
				};
				pm.project_log.Add(pl);
				pm.SaveChanges();
			}
			catch
			{
				throw;
			}


		}
	}
}