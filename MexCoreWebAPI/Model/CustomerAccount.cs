namespace MexCoreWebAPI.Model
{ 
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CustomerAccount")]
    public partial class CustomerAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerAccount()
        {
            DrCrs = new HashSet<DrCr>();
        }

        [Key]
        public int AccountID { get; set; }

        public int? PaymentModeID { get; set; }

        public int? CustomerID { get; set; }

        public int? CurrencyID { get; set; }

        [Column(TypeName = "money")]
        public decimal? CreditLimit { get; set; }

        [Column(TypeName = "money")]
        public decimal? GaurantedAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(70)]
        public string DocumentUrl { get; set; }

        public bool? IsActive { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual PaymentMode PaymentMode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DrCr> DrCrs { get; set; }
    }
}
