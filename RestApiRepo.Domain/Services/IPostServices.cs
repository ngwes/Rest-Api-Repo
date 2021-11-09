using Rest_Api_Repo.Domain.Requests.V1;
using Rest_Api_Repo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Domain.Services
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(Post post);
        Task<bool> DeletePostAsync(Guid postId);
        Task<Post> GetPostByIdAsync(Guid id);
        Task<List<Post>> GetPostsAsync(GetAllPostFilter filter, PaginationFilter paginationFilter);
        Task<bool> UpdatePostAsync(Post postToUpdate);
        Task<bool> UserOwnsPostAsync(Guid id, string v);
    }
}
