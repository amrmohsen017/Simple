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
    
    public partial class city
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public city()
        {
            this.stations = new HashSet<station>();
            this.institute_address = new HashSet<institute_address>();
        }
    
        public int citycode { get; set; }
        public string cityname { get; set; }
        public Nullable<int> governmentcode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<station> stations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<institute_address> institute_address { get; set; }
        public virtual governmnet governmnet { get; set; }
    }
}
