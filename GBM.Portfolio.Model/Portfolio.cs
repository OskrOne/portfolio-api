namespace GBM.Portfolio.Domain.Models
{
    public class Portfolio
    {
        public string ContractId { get; set; }
        public decimal BuyingPower { get; set; }
        public Asset[] Assets { get; set; }
    }
}
