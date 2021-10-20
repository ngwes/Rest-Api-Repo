using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Services
{
    public interface ITagService
    {
        IEnumerable<Tag> GetTagsById(IList<Guid> tagsIds);
        Task<IEnumerable<Tag>> GetTagsAsync();
    }
}
