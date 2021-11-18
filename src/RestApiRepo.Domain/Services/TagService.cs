using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Services
{
    public class TagService : ITagService
    {
        //private readonly DataContext _dataContext;
        private readonly ITagRepository _tagRepository;
        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        //public TagService(DataContext dataContext)  
        //{
        //    _dataContext = dataContext;
        //}

        public async Task<IEnumerable<Tag>> GetTagsByIdAsync(IList<Guid> tagIds)
        {
            var tags = await _tagRepository.GetAllTagsAsync(x => tagIds.Contains(x.Id));
            //return _dataContext.Tags.Where(x => tagIds.Any(y=>x.Id.Equals(y)));
            return tags.ToList();
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await _tagRepository.GetAllTagsAsync();
        }
    }
}
