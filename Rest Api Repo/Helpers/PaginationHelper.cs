using Rest_Api_Repo.Contracts.V1.Responses;
using Rest_Api_Repo.Domain;
using Rest_Api_Repo.Services;
using RestApi_Contracts.V1.Responses;
using System.Collections.Generic;
using System.Linq;

namespace Rest_Api_Repo.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<PostResponse> CreatePaginatedPostResponse(IUriService uriService
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
