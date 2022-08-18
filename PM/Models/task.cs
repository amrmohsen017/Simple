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
    
    public partial class task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public task()
        {
            this.blocklist_tasks = new HashSet<blocklist_tasks>();
            this.blocklist_tasks1 = new HashSet<blocklist_tasks>();
            this.task_assignedemployee = new HashSet<task_assignedemployee>();
            this.task_attachments = new HashSet<task_attachments>();
            this.task_logs = new HashSet<task_logs>();
            this.task_tags = new HashSet<task_tags>();
            this.task_access = new HashSet<task_access>();
        }
    
        public int task_id { get; set; }
        public string task_name { get; set; }
        public Nullable<System.DateTime> task_planned_start { get; set; }
        public Nullable<System.DateTime> task_planned_end { get; set; }
        public Nullable<System.DateTime> task_deadline { get; set; }
        public Nullable<int> sub_task { get; set; }
        public Nullable<int> task_supervisor { get; set; }
        public string task_description { get; set; }
        public Nullable<int> task_status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<blocklist_tasks> blocklist_tasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<blocklist_tasks> blocklist_tasks1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<task_assignedemployee> task_assignedemployee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<task_attachments> task_attachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<task_logs> task_logs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<task_tags> task_tags { get; set; }
        public virtual user user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<task_access> task_access { get; set; }
    }
}
