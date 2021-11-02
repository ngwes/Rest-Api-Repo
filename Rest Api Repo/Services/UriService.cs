using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Rest_Api_Repo.Contracts.V1;
using Rest_Api_Repo.Domain;
using System;

namespace Rest_Api_Repo.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        private readonly IHttpContextAccessor _accessor;
        public UriService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;

            var request = _accessor.HttpContext.Request;
            _baseUri = $"{request.Scheme}://{request.Host.ToUriComponent()}/";
        }

        public Uri GetPostUri(string postId)
        {
            return new Uri($"{_baseUri}/{ApiRoutes.Posts.PostBase}/{postId}");
        }

        public Uri GetAllPostUrl(PaginationFilter pagination = null)
        {
            if (pagination is null)
                return new Uri(_baseUri);

            var uri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", pagination.PageNumber.ToString());
            uri = QueryHelpers.AddQueryString(uri, "pageSize", pagination.PageSize.ToString());
            return new Uri(uri);
        }
    }
}
