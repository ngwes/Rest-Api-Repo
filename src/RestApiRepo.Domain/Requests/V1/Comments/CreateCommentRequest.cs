using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Requests.V1.Comments
{
    public class CreateCommentRequest
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
    }
}
