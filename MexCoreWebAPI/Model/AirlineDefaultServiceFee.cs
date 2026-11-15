using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MexCoreWebAPI.Model
{
    [Table("AirlineDefaultServiceFee")]
    public class AirlineDefaultServiceFee
    {
        [Key]
        public int AirlineServiceFeeID { get; set; }

        [StringLength(3)]
        public string PassengerType { get; set; }

        [StringLength(20)]
        public string ServiceFeeType { get; set; }

        public double? Amount { get; set; }
    }
}
