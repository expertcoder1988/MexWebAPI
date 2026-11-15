namespace MexCoreWebAPI.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("PaymentMode")]
    public partial class PaymentMode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PaymentMode()
        {
            CustomerAccounts = new HashSet<CustomerAccount>();
        }

        public int PaymentModeID { get; set; }

        [Column("PaymentMode")]
        [StringLength(20)]
        public string PaymentMode1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerAccount> CustomerAccounts { get; set; }
    }
}
