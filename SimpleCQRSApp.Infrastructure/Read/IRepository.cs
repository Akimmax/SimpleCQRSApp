using System.Threading.Tasks;

namespace SimpleCQRSApp.Infrastructure.Read
{
    public interface IRepository<T> : IReadOnlyRepository<T>
    where T : IReadEntity
    {
        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);
    }
}