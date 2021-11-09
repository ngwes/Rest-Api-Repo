using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Domain.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IdentityUser User {get;set;}
        public IList<PostTag> PostTags { get; set; }
        public string UserId {get;set;}
    }
}
