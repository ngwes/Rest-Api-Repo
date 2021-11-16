using RestApiRepo.Domain.Entities;
using System;

namespace RestApiRepo.Helpers
{
    public interface IPostUriBuilder
    {
        Uri GetPostUri(string postId);
        Uri GetAllPostUrl(PaginationFilter pagination = null);
    }
}
