//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class project_updates
    {
        public int update_id { get; set; }
        public string update_name { get; set; }
        public System.DateTime update_date { get; set; }
        public int update_status { get; set; }
        public float update_progress { get; set; }
        public Nullable<int> update_author_id { get; set; }
        public string update_description { get; set; }
        public int project_id { get; set; }
    
        public virtual project project { get; set; }
        public virtual user user { get; set; }
        public virtual status status { get; set; }
    }
}
