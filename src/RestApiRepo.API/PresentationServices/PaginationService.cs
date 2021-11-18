using Microsoft.AspNetCore.Http;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Extensions;
using RestApiRepo.ResponseModels;
using System.Collections.Generic;
using System.Linq;

namespace RestApiRepo.PresentationServices
{
    public class PaginationService : IPaginationService
    {
        private readonly IUriBuilderFactory _uriBuilerFactory;
        private readonly IHttpContextAccessor _accessor;
        private readonly string _baseUri;

        public PaginationService(IUriBuilderFactory uriBuilerFactory, IHttpContextAccessor accessor)
        {
            _uriBuilerFactory = uriBuilerFactory;
            _accessor = accessor;
            _baseUri = _accessor.HttpContext.GetBaseUri();
        }

        public PagedResponse<T> CreatePaginatedResponse<T>(PaginationFilter pagination, List<T> response)
        {
            if (pagination is null || pagination.PageNumber < 0 || pagination.PageSize < 1)
                return new PagedResponse<T>
                {
                    Data = response,
                };

            var uriBuilder = _uriBuilerFactory.CreateBuilder<T>();
            var nextPage =
                pagination.PageNumber >= 1 ?
                uriBuilder.GetAllRecordsUrl(_baseUri, new PaginationFilter(pagination).AddAPage())
                : null;
            var previousPage = pagination.PageNumber - 1 >= 1 ?
                uriBuilder.GetAllRecordsUrl(_baseUri, new PaginationFilter(pagination).RemoveAPage())
                : null;
            return new PagedResponse<T>
            {
                Data = response,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null
            };
        }
    }
}
