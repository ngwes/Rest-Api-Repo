using Rest_Api_Repo.Domain;
using System;

namespace Rest_Api_Repo.Services
{
    public interface IUriService
    {
        Uri GetPostUri(string postId);
        Uri GetAllPostUrl(PaginationFilter pagination = null);
    }
}
