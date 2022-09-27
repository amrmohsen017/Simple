using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PM.Models;

namespace PM.ViewModels
{
    public class projectMoreData
    {
        public List<institute> instituties { get; set; }
        public List<project_stage> stages { get; set; }
        public List<status> status { get; set; }

        public List<gross_marign_type> gross_marign_type { get; set; }
        public int project_id { get; set; }
        [RegularExpression(@"^[a-zA-z\u0621-\u064A ]+[a-zA-z0-9\u0621-\u064A\u0660-\u0669]+", ErrorMessage = "Numbers only not allowed!")]
        [Required]
        public string projectname { get; set; }
        public DateTime plannedstartdate { get; set; }
        public DateTime plannedenddate { get; set; }
        [Required]
        public string description { get; set; }
        public double cost { get; set; }
        public int? institute_id { get; set; }
        public int? project_manager_id { get; set; }
        public DateTime deadline_date { get; set; }
        public int project_stage_id { get; set; }
        public int? client { get; set; }
        public int project_status { get; set; }
        public string duplication { get; set; }

        public string gm { get; set; }
        public List<WhatIneed> funder { get; set; }
        public class WhatIneed {

            public int funder_id;
            public string funder_name; 

        }
		public projectMoreData()
		{
			project_managementEntities1 pm = new project_managementEntities1();
			pm.Configuration.ProxyCreationEnabled = false;
			instituties = pm.institutes.ToList();
			stages = pm.project_stage.ToList();
			status = pm.status.ToList();
            gross_marign_type = pm.gross_marign_type.ToList();
            funder = pm.users.Select(u => new WhatIneed { funder_id=u.user_id, funder_name=u.username }).ToList();

        }
	}
}