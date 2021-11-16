using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Requests.V1.Posts
{
    public class GetAllPostsQuery
    {
        public string UserId { get; set; }
    }
}
