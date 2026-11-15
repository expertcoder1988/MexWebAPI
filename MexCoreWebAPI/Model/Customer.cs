namespace MexCoreWebAPI.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Customer")]
    public partial class Customer
    {

        public int CustomerID { get; set; }

        [StringLength(150)]
        public string? CompanyName { get; set; }

        [StringLength(50)]
        public string? RegestrationNumber { get; set; }

        [StringLength(50)]
        public string? LicenceNumber { get; set; }

        [StringLength(30)]
        public string? StreetAddress { get; set; }

        [StringLength(150)]
        public string? AddressLine1 { get; set; }

        [StringLength(150)]
        public string? Addressline2 { get; set; }

        [StringLength(50)]
        public string? PostalCode { get; set; }

        [StringLength(20)]
        public string? Fax { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? Website { get; set; }

        public int? CityID { get; set; }

        [StringLength(20)]
        public string? Telephone { get; set; }

        public byte? Status { get; set; }

        public DateTime? DateCreated { get; set; }

        public int? CreatedBy { get; set; }

        public int? ParentID { get; set; }

        [StringLength(150)]
        public string? ContactPerson { get; set; }

        [StringLength(50)]
        public string? CustomerCode { get; set; }

        [StringLength(100)]
        public string? LogoPath { get; set; }

        [StringLength(50), Display(Name = "Customer Mask")]
        public string? CustomerMask { get; set; }

        //public virtual ICollection<CustomerAccount> CustomerAccounts { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Incentive> Incentives { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<User> Users { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<User> Users1 { get; set; }

    }
}
