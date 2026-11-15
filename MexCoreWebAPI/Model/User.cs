namespace MexCoreWebAPI.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    [Table("User")]
    public class User
    {
     
        public int UserID { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? MiddleName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(250)]
        public string? AddressLine1 { get; set; }

        [StringLength(250)]
        public string? AddressLine2 { get; set; }

        public int? CityID { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        public bool? IsAdmin { get; set; }
        public bool? Gender { get; set; }

        [StringLength(50)]
        public string? MobileNumber { get; set; }

        [StringLength(50)]
        public string? Telephone { get; set; }

        public byte? Status { get; set; }

        [StringLength(50)]
        public string? UserName { get; set; }

        [StringLength(300)]
        public string? Password { get; set; }

        public DateTime? DateCreated { get; set; }

        public int? CreatedBy { get; set; }

        [StringLength(20)]
        public string? ActivationCode { get; set; }

        [ForeignKey("CustomerID")]
        public int? CustomerID { get; set; }

        public byte? UserTypeID { get; set; }

        [Column(TypeName = "money")]
        public decimal? POSMarkUp { get; set; }

        [StringLength(10)]
        public string? POSMarkupType { get; set; }

        public byte? AccessType { get; set; }

        [StringLength(50)]
        public string? ProfilePic { get; set; }
        public bool? PayNow { get; set; }
        public bool? PayLater { get; set; }
        public decimal? PayLaterMarkup { get; set; }

    }
}
