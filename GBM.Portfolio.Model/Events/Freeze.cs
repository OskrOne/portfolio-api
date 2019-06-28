namespace GBM.Portfolio.Domain.Models.Events
{
    public class Freeze : Event
    {
        public string InstrumentId { get; set; }
        public string InstrumentName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Side Side { get; set; }
    }

    public enum Side { Buy, Sell };
}
