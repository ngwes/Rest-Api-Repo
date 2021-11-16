using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Repositories
{
    public interface ITagRepository : IRepository 
    {
        public Task<IEnumerable<Tag>> GetAllTagsAsync(
           Expression<Func<Tag, bool>> filter = null,
           Func<IQueryable<Tag>, IOrderedQueryable<Tag>> orderBy = null,
           string includeProperties = "",
           int skip = 0,
            int take = 0);
        public Task<Tag> GetTagByIdAsync(Guid id);
        public void InsertTag(Tag entity);
        public Task DeleteTagByIdAsync(Guid id);
        public void DeleteTag(Tag entityToDelete);
        public void UpdateTag(Tag entityToUpdate);
    }
}
