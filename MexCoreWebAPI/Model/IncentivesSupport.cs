namespace MexCoreWebAPI.Model
{ 
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("IncentivesSupport")]
    public partial class IncentivesSupport
    {
        [Key]
        public int IncentiveSupportID { get; set; }

        [StringLength(50)]
        public string IncentiveType { get; set; }

        [Column(TypeName = "money")]
        public decimal? Incentives { get; set; }

        [StringLength(10)]
        public string Scale { get; set; }

        [Column(TypeName = "money")]
        public decimal? RefIncentives { get; set; }

        [StringLength(10)]
        public string RefScale { get; set; }

        public int? ServiceID { get; set; }

        public byte? UserTypeID { get; set; }

        public virtual UserType UserType { get; set; }
    }
}
