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
    
    public partial class project_tag
    {
        public int id { get; set; }
        public int tag_id { get; set; }
        public int project_id { get; set; }
    
        public virtual project project { get; set; }
        public virtual tag tag { get; set; }
    }
}
