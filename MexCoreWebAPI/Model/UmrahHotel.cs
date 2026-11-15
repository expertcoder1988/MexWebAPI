using System.ComponentModel.DataAnnotations;



namespace MexCoreWebAPI.Model
{
    public class UmrahHotel
    {
        [Key]
        public int UH_ID { get; set; }

        [StringLength(50)]
        public string C_Name { get; set; }

        [StringLength(300)]
        public string U_Hotels { get; set; }

        [StringLength(300)]
        public string Cat_Name { get; set; }

        [StringLength(300)]
        public string Location { get; set; }

        [StringLength(300)]
        public string DistanceMeter { get; set; }

        public int? SharingORQuint { get; set; }

        public int? Quad { get; set; }

        public int? Triple { get; set; }

        public int? Doublee { get; set; }

        public string Images { get; set; }
    }
}
