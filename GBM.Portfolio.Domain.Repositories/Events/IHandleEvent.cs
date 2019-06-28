using GBM.Portfolio.Domain.Models.Events;

namespace GBM.Portfolio.Domain.Repositories.Events
{
    public interface IHandleEvent
    {
        void Handle(Event @event);
    }
}
