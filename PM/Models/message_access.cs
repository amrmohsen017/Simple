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
    
    public partial class message_access
    {
        public int ID { get; set; }
        public int message_id { get; set; }
        public int user_id { get; set; }
        public int access_type { get; set; }
    
        public virtual message message { get; set; }
        public virtual user user { get; set; }
    }
}
