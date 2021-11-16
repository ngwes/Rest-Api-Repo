using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Responses.V1;
using RestApiRepo.Domain.Services;
using RestApiRepo.ResponseModels;
using System.Collections.Generic;
using System.Linq;

namespace RestApiRepo.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<PostResponse> CreatePaginatedPostResponse(IPostUriBuilder uriService
            , PaginationFilter pagination, List<PostResponse> response)
        {
            var nextPage =
                pagination.PageNumber >= 1 ?
                uriService.GetAllPostUrl(new PaginationFilter(pagination).AddAPage()).ToString()
                : null;
            var previousPage = pagination.PageNumber - 1 >= 1 ?
                uriService.GetAllPostUrl(new PaginationFilter(pagination).RemoveAPage()).ToString()
                : null;
            return new PagedResponse<PostResponse>
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
