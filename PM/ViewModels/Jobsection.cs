using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PM.Models;
using PagedList;
namespace PM.ViewModels
{
    public class Jobsection
    {
        public job jobs { get; set; }
        public section sections { get; set; }

        public List<section> sections2 { get; set; }
        public int section_id { get; set; }
        public   List<job>jobs2 { get; set; }
    }
}