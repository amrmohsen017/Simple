using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PM.ViewModels
{
	public class UserView
	{
		public int user_id { get; set; }

		public string username { get; set; }

		public string telephone { get; set; }

		public string email { get; set; }

		public int? job_id { get; set; }

		public int institute_id { get; set; }

		public int level_code { get; set; }

		public string pass { get; set; }


	}
}