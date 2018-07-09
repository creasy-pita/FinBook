using System;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Domain.SeedWork
{
    /// <summary>
    /// 可以用来统一事务操作
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
