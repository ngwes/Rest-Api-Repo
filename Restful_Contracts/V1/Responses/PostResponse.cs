using System;
using System.Collections.Generic;

namespace Rest_Api_Repo.Contracts.V1.Responses
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public IEnumerable<ResponseTag> Tags { get; set; }

    }
}
