using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PM.Models;

namespace PM.ViewModels
{
	public class projectsPanelView
	{
		public class projectView
		{
			public int project_id { get; set; }
			public string projectname { get; set; }
			public DateTime? plannedstartdate { get; set; }
			public DateTime? plannedenddate { get; set; }
			public string description { get; set; }
			public double? cost { get; set; }
			public int? institute_id { get; set; }

			public int? project_manager_id { get; set; }
			public DateTime? deadline_date { get; set; }
			public int? project_stage_id { get; set; }
			public int? client { get; set; }
			public int project_status { get; set; }

			//all_names
			public string stage_name { get; set; }
			public string client_name { get; set; }
			public string status_name { get; set; }
			public string institute_name { get; set; }
			public string projectManager_name { get; set; }


			//all connected tables
			//public List<grossMarginView> gross_marign_list { get; set; }

			//public List<logView> logs_list { get; set; }
			//public List<projectAttchment> files { get; set; }		

		}


		public List<projectView> all_projects;

		public Dictionary<string, projectView> projects = new Dictionary<string, projectView>();
		public List<stagesView> stages { get; set; }

		public List<institutesView> institutes { get; set; }



		public projectsPanelView()
		{
			project_managementEntities1 pm = new project_managementEntities1();

			 all_projects = (from p in pm.projects
								select new projectView
								{
									project_id = p.project_id,
									projectname = p.projectname,
									plannedstartdate = p.plannedstartdate,
									plannedenddate = p.plannedenddate,
									description = p.description,
									cost = p.cost,
									institute_id = p.institute_id,
									project_manager_id = p.project_manager_id,
									deadline_date = p.deadline_date,
									project_stage_id = p.project_stage_id,
									client = p.client,
									project_status = p.project_status,

									stage_name = p.project_stage.stage_name,
									institute_name = p.institute.institutename,
									status_name = p.status.status_name,
									client_name = p.user.username

									}).ToList();

									//gross_marign_list = (from g in pm.gross_marign
									//					 select new grossMarginView
									//					 {
									//						 gross_margin_id = g.gross_marign_id,
									//						 description = g.description,
									//						 gross_date = g.gross_date,
									//						 quantity = g.quantity,
									//						 Amount = g.Amount,
									//						 gross_type = g.gross_type,
									//						 user_associated = g.user_associated
									//					 }).ToList(),

									//logs_list = (from l in pm.project_log
									//			 where l.project_id == p.project_id
									//			 select new logView
									//			 {
									//				 log_id = l.log.log_id,
									//				 log_text = l.log.log_text,
									//				 log_date = l.log.log_date.ToString()

									//			 }).ToList(),

									//files = (from a in pm.project_attachment
									//		 where a.project_id == p.project_id
									//		 select new projectAttchment
									//		 {
									//			 attachment_id = a.attachemnt.attachment_id,
									//			 attachment_name = a.attachemnt.attachment_name,
									//			 attachment_path = a.attachemnt.attachment_path,
									//			 attachment_url = a.attachemnt.attachment_url

									//		 }).ToList()
								
			foreach(var p in all_projects)
			{
				projects[p.project_id.ToString()] = p;
			}

			stages = (from s in pm.project_stage
					  select new stagesView
					  {
						  stage_id = s.stage_id,
						  stage_name = s.stage_name
					  }).ToList();

			institutes = (from i in pm.institutes
						  select new institutesView
						  {
							  institute_id = i.institute_id,
							  institutename = i.institutename,
							  institute_fulladdress = i.institute_fulladdress,
							  telephone = i.telephone,
							  email = i.email,
							  type_id = i.type_id,
							  department_id = i.department_id,
							  address_id = i.adress_id


						  }).ToList();
		}
    }
}