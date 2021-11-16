using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
        public IList<PostTag> PostTags { get; set; }
        public string UserCreatorId { get; set; }
        public IdentityUser UserCreator { get; set; }
        public DateTime CreatedAt{ get; set; }
    }
}
