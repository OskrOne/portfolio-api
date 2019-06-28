namespace GBM.Portfolio.Domain.Models.Events
{
    public class AddMoney : Event
    {
        public decimal Money { get; set; }

        public AddMoney() {
        }
    }
}
