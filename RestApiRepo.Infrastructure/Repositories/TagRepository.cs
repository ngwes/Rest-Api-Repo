using Rest_Api_Repo.Domain.Entities;
using Rest_Api_Repo.Infrastructure;
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Infrastructure.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(DataContext context) : base(context)
        {
           
        }
        public async Task DeleteTagByIdAsync(Guid id)
        {
            await base.DeleteByIdAsync(id);
        }

        public void DeleteTag(Tag entityToDelete)
        {
            base.Delete(entityToDelete);
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync(Expression<Func<Tag, bool>> filter = null, Func<IQueryable<Tag>, IOrderedQueryable<Tag>> orderBy = null, string includeProperties = "", int skip = 0, int take = 0)
        {
            return await base.GetAsync(filter, orderBy, includeProperties, skip, take);
        }

        public async Task<Tag> GetTagByIdAsync(Guid id)
        {
            return await base.GetByIdAsync(id);
        }

        public void InsertTag(Tag entity)
        {
            base.Insert(entity);
        }

        public void UpdateTag(Tag entityToUpdate)
        {
            base.Update(entityToUpdate);
        }
    }
}
