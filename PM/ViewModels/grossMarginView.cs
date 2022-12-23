using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PM.Models;

namespace PM.ViewModels
{
	public class grossMarginView
	{
		public int gross_margin_id { get; set; }
		public string description { get; set; }
		public DateTime gross_date { get; set; }
		public string gross_date_string { get; set; }
		public double? quantity { get; set; }
		public double? Amount { get; set; }

		public string project_name { get; set; }
		public string funder { get; set; }
		public string gross_margin_typename { get; set; }
		public int project_id { get; set; }

		public int? gross_type { get; set; }

		public int? user_associated { get; set; }

	}
}