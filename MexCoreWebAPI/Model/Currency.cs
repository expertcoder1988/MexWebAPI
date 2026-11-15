namespace MexCoreWebAPI.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Currency")]
    public partial class Currency
    {
     
        public Currency()
        {
            CustomerAccounts = new HashSet<CustomerAccount>();
        }

        public int CurrencyID { get; set; }

        [StringLength(50)]
        public string CurrencyCode { get; set; }

        [Required]
        [StringLength(50)]
        public string CurrencyName { get; set; }

        [Required]
        [StringLength(50)]
        public string CoinsName { get; set; }

        [Column(TypeName = "money")]
        public decimal ExchangeRate { get; set; }

        public bool IsBase { get; set; }


        public virtual ICollection<CustomerAccount> CustomerAccounts { get; set; }
    }
}
