using Rest_Api_Repo.Domain.Entities;
using System;

namespace Rest_Api_Repo.Helpers
{
    public interface IPostUriBuilder
    {
        Uri GetPostUri(string postId);
        Uri GetAllPostUrl(PaginationFilter pagination = null);
    }
}
