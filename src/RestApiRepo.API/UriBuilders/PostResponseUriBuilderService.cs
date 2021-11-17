using Microsoft.AspNetCore.WebUtilities;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Responses.V1;
using RestApiRepo.PresentationServices;
using RestApiRepo.Routes.V1.ApiRoutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.UriBuilders
{
    public class PostResponseUriBuilderService : IUriBuilderService
    {
        public Type ResourceType { get; } = typeof(PostResponse);

        public string GetAllRecordsUrl(string baseUri, PaginationFilter paginationFilter)
        {
            if (paginationFilter is null)
                return baseUri;
            var uri = $"{baseUri}/{ApiRoutes.Posts.PostBase}/{ApiRoutes.Posts.GetAll}";
            uri = QueryHelpers.AddQueryString(uri, "pageNumber", paginationFilter.PageNumber.ToString());
            uri = QueryHelpers.AddQueryString(uri, "pageSize", paginationFilter.PageSize.ToString());
            return uri;
        }

        public string GetRecordByIdUrl(string baseUri, string id)
        {
            return $"{baseUri}/{ApiRoutes.Posts.PostBase}/{id}";
        }
    }
}
