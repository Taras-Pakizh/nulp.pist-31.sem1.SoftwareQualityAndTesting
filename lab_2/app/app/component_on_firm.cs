//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace app
{
    using System;
    using System.Collections.Generic;
    
    public partial class component_on_firm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public component_on_firm()
        {
            this.transmission_component_install = new HashSet<transmission_component_install>();
        }
    
        public int id_component_on_firm { get; set; }
        public int component { get; set; }
        public int firm { get; set; }
        public int component_count { get; set; }
    
        public virtual component component1 { get; set; }
        public virtual firm firm1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<transmission_component_install> transmission_component_install { get; set; }
    }
}
