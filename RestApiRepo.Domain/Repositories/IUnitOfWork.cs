using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Repositories
{

    public interface IUnitOfWork : IDisposable
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }

}
