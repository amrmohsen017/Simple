using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PM.ViewModels
{
	public class projectUpdatesView
	{
		public int update_id { get; set; }

		public string update_name { get; set; }

		public DateTime update_date { get; set; }

		public string update_date_string { get; set; }

		public int status { get; set; }

		public string status_name { get; set; }

		public float progress { get; set; }
		public int? updater { get; set; }
		public string updater_name { get; set; }
		public string description { get; set; }

		public int project_id { get; set; }



	}
}