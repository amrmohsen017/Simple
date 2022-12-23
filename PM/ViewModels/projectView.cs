using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PM.ViewModels
{
	public class projectView
	{
		public int project_id { get; set; }
		public string projectname { get; set; }
		public DateTime? plannedstartdate { get; set; }
		public DateTime? plannedenddate { get; set; }
		public string plannedstartdate_string { get; set; }
		public string plannedenddate_string { get; set; }
		public string description { get; set; }
		public double? cost { get; set; }
		public int? institute_id { get; set; }

		public int? project_manager_id { get; set; }
		public DateTime? deadline_date { get; set; }
		public string deadline_date_string { get; set; }
		public int? project_stage_id { get; set; }
		public int? client { get; set; }
		public int project_status { get; set; }

		//all_names
		public string stage_name { get; set; }
		public string client_name { get; set; }
		public string status_name { get; set; }
		public string institute_name { get; set; }
		public string projectManager_name { get; set; }

		public List<tagView> project_tags = new List<tagView>();

		public List<projectUpdatesView> project_update_progress = new List<projectUpdatesView>();


		//all connected tables
		//public List<grossMarginView> gross_marign_list { get; set; }

		//public List<logView> logs_list { get; set; }
		//public List<projectAttchment> files { get; set; }		
	}
}