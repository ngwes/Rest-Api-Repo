using System;
using System.Collections.Generic;

namespace RestApiRepo.Domain.Responses.V1
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public IEnumerable<ResponseTag> Tags { get; set; }

    }
}
