using Rest_Api_Repo.Domain.Entities;
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Domain.Services
{

    public class PostService : IPostService
    {

        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            _postRepository.InsertPost(post);
            return await _postRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post is null)
                return false;
            _postRepository.DeletePost(post);
            return await _postRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            var posts = await _postRepository.GetAllPostsAsync(x => x.Id.Equals(id), null, "PostTags.Tag");
            return posts.FirstOrDefault();
        }

        public async Task<List<Post>> GetPostsAsync(GetAllPostFilter filter, PaginationFilter paginationFilter = null)
        {
            var skip = paginationFilter.PageNumber <= 1 ? 0 : paginationFilter.PageNumber * paginationFilter.PageSize;
            
            IEnumerable<Post> posts;
            if (!string.IsNullOrEmpty(filter?.UserId))
                posts = await _postRepository.GetAllPostsAsync(x => x.UserId.Equals(filter.UserId), null, "PostTags.Tag", skip, paginationFilter.PageSize);
            else
            {
                posts = await _postRepository.GetAllPostsAsync(null, null, "PostTags.Tag", skip, paginationFilter.PageSize);
            }
            return posts.ToList();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            _postRepository.UpdatePost(postToUpdate);
            return await _postRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post is null)
                return false;
            return post.UserId.Equals(userId);
        }
    }
}
