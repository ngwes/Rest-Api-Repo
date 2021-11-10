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

        //private readonly DataContext _dataContext;
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            //await _dataContext.Posts.AddAsync(post);
            //var created = await _dataContext.SaveChangesAsync();
            _postRepository.InsertPost(post);
            return await _postRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            //var post = await GetPostByIdAsync(postId);
            if (post is null)
                return false;
            _postRepository.DeletePost(post);
            //_dataContext.Posts.Remove(post);
            //var deleted = await _dataContext.SaveChangesAsync();
            //return deleted > 0;
            return await _postRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            //return await _dataContext.Posts
            //    .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
            //    .Include(p => p.User)
            //    .SingleOrDefaultAsync(p => p.Id.Equals(id));
            var posts = await _postRepository.GetAllPostsAsync(x => x.Id.Equals(id), null, "PostTags.Tag");
            return posts.FirstOrDefault();
        }

        public async Task<List<Post>> GetPostsAsync(GetAllPostFilter filter, PaginationFilter paginationFilter = null)
        {
            //var queryable = _dataContext.Posts
            //    .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
            //    .Include(x => x.User)
            //    .AsQueryable();

            //if (paginationFilter is null)
            //    return await queryable.ToListAsync();

            var skip = paginationFilter.PageNumber <= 1 ? 0 : paginationFilter.PageNumber * paginationFilter.PageSize;
            //if (skip < 0)
            //    return new List<Post>();

            //queryable = AddFilterOnQuery(filter, queryable);

            //return await queryable
            //        .Skip(skip)
            //        .Take(paginationFilter.PageSize)
            //        .ToListAsync();
            IEnumerable<Post> posts;
            if (!string.IsNullOrEmpty(filter?.UserId))
                posts = await _postRepository.GetAllPostsAsync(x => x.UserId.Equals(filter.UserId), null, "PostTags.Tag", skip, paginationFilter.PageSize);
            else
            {
                posts = await _postRepository.GetAllPostsAsync(null, null, "PostTags.Tag", skip, paginationFilter.PageSize);
            }
            return posts.ToList();
        }

        //private static IQueryable<Post> AddFilterOnQuery(GetAllPostFilter filter, IQueryable<Post> queryable)
        //{
        //    if (!string.IsNullOrEmpty(filter?.UserId))
        //        queryable = queryable.Where(p => p.UserId.Equals(filter.UserId));
        //    return queryable;
        //}

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            //_dataContext.Posts.Update(postToUpdate);
            //var updatedCount = await _dataContext.SaveChangesAsync();

            //return updatedCount > 0;
            _postRepository.UpdatePost(postToUpdate);
            return await _postRepository.UnitOfWork.SaveEntitiesAsync();
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            //var post = await _dataContext.Posts
            //    .AsNoTracking()
            //    .SingleOrDefaultAsync(p => p.Id.Equals(postId));
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post is null)
                return false;
            return post.UserId.Equals(userId);
        }
    }
}
