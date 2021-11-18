using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Repositories
{
    public interface IPostRepository : IRepository
    {
        public Task<IEnumerable<Post>> GetAllPostsAsync(
           Expression<Func<Post, bool>> filter = null,
           Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null,
           string includeProperties = "",
           int skip = 0,
            int take = 0);
        public Task<Post> GetPostByIdAsync(Guid id);
        public void InsertPost(Post entity);
        public Task DeletePostByIdAsync(Guid id);
        public void DeletePost(Post entityToDelete);
        public void UpdatePost(Post entityToUpdate);
    }
}
