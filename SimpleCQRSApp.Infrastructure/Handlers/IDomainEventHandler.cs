using SimpleCQRSApp.Infrastructure.Core;
using System.Threading.Tasks;

namespace SimpleCQRSApp.Infrastructure.Handlers
{
    public interface IDomainEventHandler<TAggregateId, TEvent>
        where TEvent: IDomainEvent<TAggregateId>
    {
        Task HandleAsync(TEvent @event);
    }
}
