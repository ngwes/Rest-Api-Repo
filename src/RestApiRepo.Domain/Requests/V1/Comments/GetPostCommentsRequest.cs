using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Requests.V1.Comments
{
    public class GetPostCommentsRequest
    {
        public Guid PostId { get; set; }
    }
}
