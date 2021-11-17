using RestApiRepo.Domain.Entitites;
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(DataContext context) : base(context)
        {
        }

        public async Task DeleteCommentByIdAsync(Guid id)
        {
            await base.DeleteByIdAsync(id);
        }

        public void DeleteComment(Comment entityToDelete)
        {
            base.Delete(entityToDelete);
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync(Expression<Func<Comment, bool>> filter = null, Func<IQueryable<Comment>, IOrderedQueryable<Comment>> orderBy = null, string includeProperties = "", int skip = 0, int take = 0)
        {
            return await base.GetAsync(filter, orderBy, includeProperties, skip, take);
        }

        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return await base.GetByIdAsync(id);
        }

        public void InsertComment(Comment entity)
        {
            base.Insert(entity);
        }

        public void UpdateComment(Comment entityToUpdate)
        {
            base.Update(entityToUpdate);
        }
    }
}
