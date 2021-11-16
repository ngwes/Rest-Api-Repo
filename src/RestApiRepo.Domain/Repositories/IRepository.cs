using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Repositories
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
