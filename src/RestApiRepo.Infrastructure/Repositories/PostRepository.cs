using RestApiRepo.Domain.Entities;
 
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(DataContext context) : base(context)
        {
        }

        public async Task DeletePostByIdAsync(Guid id)
        {
            await base.DeleteByIdAsync(id);
        }

        public void DeletePost(Post entityToDelete)
        {
            base.Delete(entityToDelete);
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync(Expression<Func<Post, bool>> filter = null, 
            Func<IQueryable<Post>, IOrderedQueryable<Post>> orderBy = null,
            string includeProperties = "",
           int skip = 0,
            int take = 0)
        {
            return await base.GetAsync(filter, orderBy, includeProperties, skip, take);
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            return await base.GetByIdAsync(id);
        }

        public void InsertPost(Post entity)
        {
            base.Insert(entity);
        }

        public void UpdatePost(Post entityToUpdate)
        {
            base.Update(entityToUpdate);
        }
    }
}
