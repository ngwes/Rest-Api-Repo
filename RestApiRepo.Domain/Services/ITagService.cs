using Rest_Api_Repo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Domain.Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetTagsByIdAsync(IList<Guid> tagsIds);
        Task<IEnumerable<Tag>> GetTagsAsync();
    }
}
