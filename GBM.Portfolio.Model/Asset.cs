namespace GBM.Portfolio.Model
{
    public class Asset
    {
        public string ContractId { get; set; }
        public string InstrumentId { get; set; }
        public int? Quantity { get; set; }
        public decimal? AveragePrice { get; set; }
        public decimal? LastPrice { get; set; }
        public decimal? ClosePrice { get; set; }
    }
}
