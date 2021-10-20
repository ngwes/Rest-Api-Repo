using Microsoft.EntityFrameworkCore;
using Rest_Api_Repo.Data;
using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Services
{
    public class TagService : ITagService
    {
        private readonly DataContext _dataContext;

        public TagService(DataContext dataContext)  
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Tag> GetTagsById(IList<Guid> tagIds)
        {
            return _dataContext.Tags.Where(x => tagIds.Any(y=>x.Id.Equals(y)));
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _dataContext.Tags.ToListAsync();
        }
    }
}
