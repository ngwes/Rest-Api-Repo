using RestApiRepo.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Repositories
{
    public interface ICommentRepository : IRepository
    {
        public Task<IEnumerable<Comment>> GetAllCommentsAsync(
           Expression<Func<Comment, bool>> filter = null,
           Func<IQueryable<Comment>, IOrderedQueryable<Comment>> orderBy = null,
           string includeProperties = "",
           int skip = 0,
            int take = 0);
        public Task<Comment> GetCommentByIdAsync(Guid id);
        public void InsertComment(Comment entity);
        public Task DeleteCommentByIdAsync(Guid id);
        public void DeleteComment(Comment entityToDelete);
        public void UpdateComment(Comment entityToUpdate);
    }
}
