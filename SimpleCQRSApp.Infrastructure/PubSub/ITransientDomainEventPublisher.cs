using System.Threading.Tasks;

namespace SimpleCQRSApp.Infrastructure.PubSub
{
    public interface ITransientDomainEventPublisher
    {
        Task PublishAsync<T>(T publishedEvent);
    }
}