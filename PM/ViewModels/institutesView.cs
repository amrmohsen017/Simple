using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PM.ViewModels
{
	public class institutesView
	{
		public int institute_id { get; set; }
		public string institutename { get; set; }
		public string institute_fulladdress { get; set; }
		public string telephone { get; set; }
		public string email { get; set; }
		public int? type_id { get; set; }
		public int? department_id { get; set; }
		public int? address_id { get; set; }
	}
}