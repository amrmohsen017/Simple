using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PM.Models;

namespace PM.ViewModels
{
	public class projectAttchment
	{
		//public project project { get; set; }
		public string project_name { get; set; }
		public int project_id { get; set; }
		public string attachment_name { get; set; }
		public string attachment_path { get; set; }
		public string attachment_url { get; set; }
		public int attachment_id { get; set; }

		
		
	}
}