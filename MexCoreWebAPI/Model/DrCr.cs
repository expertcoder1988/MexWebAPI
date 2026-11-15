namespace MexCoreWebAPI.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DrCr")]
    public partial class DrCr
    {
        public int DrCrID { get; set; }

        [Column(TypeName = "money")]
        public decimal? Credit { get; set; }

        [Column(TypeName = "money")]
        public decimal? Debit { get; set; }

        [Column(TypeName = "money")]
        public decimal? Balance { get; set; }

        public int? AccountID { get; set; }

        public int? FlightBookingID { get; set; }


        public int? HotelBookingID { get; set; }

        public int? ByUserID { get; set; }

        public DateTime? TransactionTime { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        // 1 => Balance Transfer
        // 2 => Credit On Flight Booking
        // 3 => Debit On Flight Booking
        // 4 => Credit Referrals On Flight Booking
        // 5 => Credit On Hotel Booking
        // 6 => Debit On Hotel Booking
        // 7 => Credit Referrals On Hotel Booking
        // 8 => Debit on Blanace transfer from GSA to Agents/Members
        // 9 => Extra Incentives to Agents/Members
        public byte? TypeID { get; set; }

        public virtual CustomerAccount CustomerAccount { get; set; }

        public virtual Reservation FlightBooking { get; set; }

    //    public virtual HotelBooking HotelBooking { get; set; }

        public virtual User User { get; set; }
    }
}
