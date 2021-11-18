using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.PresentationServices
{
    public interface IUriBuilderFactory
    {
        IUriBuilderService CreateBuilder<TResource>();
    }
}
