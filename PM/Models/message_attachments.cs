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
    
    public partial class message_attachments
    {
        public int ID { get; set; }
        public int message_id { get; set; }
        public int attachmnet_id { get; set; }
    
        public virtual attachemnt attachemnt { get; set; }
        public virtual message message { get; set; }
    }
}
