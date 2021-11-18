using System;
using System.Collections.Generic;

namespace RestApiRepo.Domain.Requests.V1.Posts
{
    public class UpdatePostRequest
    {
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<string> NewTags { get; set; }
        public IList<Guid> ExistingTags { get; set; }
    }
}
