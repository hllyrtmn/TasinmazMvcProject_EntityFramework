//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TasinmazMvc.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MAHALLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MAHALLE()
        {
            this.BILGI = new HashSet<BILGI>();
        }
    
        public int ID { get; set; }
        public string AD { get; set; }
        public Nullable<int> SEMTID { get; set; }
        public string POSTAKOD { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BILGI> BILGI { get; set; }
        public virtual SEMT SEMT { get; set; }
    }
}
