using Rest_Api_Repo.Contracts.V1.Requests;
using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Services
{
    public interface IPostService
    {
        Task<bool> CreatePostAsync(Post post);
        Task<bool> DeletePostAsync(Guid postId);
        Task<Post> GetPostByIdAsync(Guid id);
        Task<List<Post>> GetPostsAsync();
        Task<bool> UpdatePostAsync(Post postToUpdate);
    }
}
