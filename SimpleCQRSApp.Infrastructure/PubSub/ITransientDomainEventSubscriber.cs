using System;
using System.Threading.Tasks;

namespace SimpleCQRSApp.Infrastructure.PubSub
{
    public interface ITransientDomainEventSubscriber
    {
        void Subscribe<T>(Action<T> handler);

        void Subscribe<T>(Func<T, Task> handler);
    }
}