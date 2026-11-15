namespace MexCoreWebAPI.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("Access")]
    public partial class Access
    {
        
        public Access()
        {
            UserAccesses = new HashSet<UserAccess>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte AccessID { get; set; }

        [StringLength(50)]
        public string AccessType { get; set; }

        [StringLength(300)]
        public string Note { get; set; }

      
        public virtual ICollection<UserAccess> UserAccesses { get; set; }
    }
}
