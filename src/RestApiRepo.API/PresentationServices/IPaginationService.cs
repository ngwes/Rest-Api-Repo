using RestApiRepo.Domain.Entities;
using RestApiRepo.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.PresentationServices
{
    public interface IPaginationService
    {
        public PagedResponse<T> CreatePaginatedResponse<T>(PaginationFilter pagination, List<T> response);
    }
}
