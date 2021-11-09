using System;
using System.Collections.Generic;

namespace Rest_Api_Repo.Domain.Requests.V1
{
    public class UpdatePostRequest
    {
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<string> NewTags { get; set; }
        public IList<Guid> ExistingTags { get; set; }
    }
}
