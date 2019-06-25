namespace GBM.Portfolio.Model
{
    public class Contract
    {
        public string ContractId { get; set; }
        public decimal BuyingPower { get; set; }
        public Asset[] Assets { get; set; }
    }
}
