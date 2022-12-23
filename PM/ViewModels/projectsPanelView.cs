using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PM.Models;

namespace PM.ViewModels
{
	public class projectsPanelView
	{

		public List<projectView> all_projects;

		public Dictionary<string, projectView> projects = new Dictionary<string, projectView>();
		public Dictionary<string, stagesView> stages_dict = new Dictionary<string, stagesView>();
		public Dictionary<string, UserView> users_dict = new Dictionary<string, UserView>();
		public Dictionary<string, grossTypes> types_dict = new Dictionary<string, grossTypes>();
		public Dictionary<string, projectStatusView> status_dict = new Dictionary<string, projectStatusView>();
		public List<stagesView> stages { get; set; }
		public List<tagView> tags { get; set; }
		public List<institutesView> institutes { get; set; }

		public List<UserView> users { get; set; }

		public List<grossTypes> types { get; set; }

		public List<projectStatusView> statuses { get; set; }



		public projectsPanelView()
		{
			project_managementEntities1 pm = new project_managementEntities1();

			all_projects = (from p in pm.projects
							orderby p.project_id
							select new projectView
							{
								project_id = p.project_id,
								projectname = p.projectname,
								plannedstartdate = p.plannedstartdate,
								plannedenddate = p.plannedenddate,
								plannedstartdate_string = p.plannedstartdate.ToString(),
								plannedenddate_string = p.plannedenddate.ToString(),
								description = p.description,
								cost = p.cost,
								institute_id = p.institute_id,
								project_manager_id = p.project_manager_id,
								deadline_date = p.deadline_date,
								deadline_date_string = p.deadline_date.ToString(),
								project_stage_id = p.project_stage_id,
								client = p.client,
								project_status = p.project_status,

								stage_name = p.project_stage.stage_name,
								institute_name = p.institute.institutename,
								status_name = p.status.status_name,
								client_name = p.user.username,

								project_tags = (from t in pm.project_tag
												where t.project_id == p.project_id
												select new tagView
												{
													tag_id = t.tag.tag_id,
													tagname = t.tag.tagname

												}).ToList(),

								project_update_progress = (from u in pm.project_updates
														   where u.project_id == p.project_id
														   orderby u.update_date descending
														   select new projectUpdatesView
														   {
															   update_id = u.update_id,
															   description = u.update_description,
															   update_name = u.update_name,
															   update_date = u.update_date,
															   update_date_string = u.update_date.ToString(),
															   status = u.update_status,
															   status_name = u.status.status_name,
															   progress = u.update_progress,
															   updater = u.update_author_id,
															   updater_name = u.user.username,
															   project_id = u.project_id
														   }).ToList()


							}).Take(2).ToList();

			
									

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

			tags = (from t in pm.tags
					  select new tagView
					  {
						  tag_id=t.tag_id,
						  tagname=t.tagname
					  }).ToList();

			users = (from u in pm.users
					 select new UserView
					 {
						 user_id = u.user_id,
						 username = u.username,
						 telephone = u.telephone,
						 email = u.email,
						 job_id = u.job_id,
						 institute_id = u.institute_id,
						 level_code = u.level_code
					 }).ToList();

			types = (from t in pm.gross_marign_type
					select new grossTypes
					{
						id = t.id,
						gross_marign_typename = t.gross_marign_typename
					}).ToList();

			statuses = (from s in pm.status
					 select new projectStatusView
					 {
						 status_id = s.status_id,
						 status_name = s.status_name
					 }).ToList();


			foreach (var s in stages)
			{
				stages_dict[s.stage_id.ToString()] = s;
			}

			foreach (var u in users)
			{
				users_dict[u.user_id.ToString()] = u;
			}

			foreach (var t in types)
			{
				types_dict[t.id.ToString()] = t;
			}

			foreach (var s in statuses)
			{
				status_dict[s.status_id.ToString()] = s;
			}


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