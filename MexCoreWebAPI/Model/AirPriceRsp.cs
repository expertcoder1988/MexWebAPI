namespace MexCoreWebAPI.Model
{
    public class AirPriceRsp
    {
        public int Id { get; set; }
        public string? TransactionId { get; set; }
        public byte[]? Response { get; set;}
    }

    public class AirPriceSol
    {
        public int Id { get; set; }
        public string? TransactionId { get; set; }
        public byte[]? Response { get; set; }
    }
}
