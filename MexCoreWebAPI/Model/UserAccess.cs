namespace MexCoreWebAPI.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("UserAccess")]
    public partial class UserAccess
    {
        public int UserAccessID { get; set; }

        public byte? AccessID { get; set; }

        public int? UserID { get; set; }

        public byte? AccessLevel { get; set; }

        public virtual Access Access { get; set; }

        public virtual User User { get; set; }
    }
}
