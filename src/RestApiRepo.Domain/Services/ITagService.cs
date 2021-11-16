using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetTagsByIdAsync(IList<Guid> tagsIds);
        Task<IEnumerable<Tag>> GetTagsAsync();
    }
}
