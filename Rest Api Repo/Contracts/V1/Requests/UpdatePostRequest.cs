using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;

namespace Rest_Api_Repo.Contracts.V1.Requests
{
    public class UpdatePostRequest
    {
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<string> NewTags { get; set; }
        public IList<Guid> ExistingTags { get; set; }
    }
}
