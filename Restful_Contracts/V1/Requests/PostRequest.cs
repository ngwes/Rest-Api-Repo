using System;
using System.Collections.Generic;

namespace Rest_Api_Repo.Contracts.V1.Requests
{
    public class PostRequest
    {
        public string Name { get; set; }
        public IList<string> NewTags { get; set; }
        public IList<Guid> ExistingTags { get; set; }
    }
}
