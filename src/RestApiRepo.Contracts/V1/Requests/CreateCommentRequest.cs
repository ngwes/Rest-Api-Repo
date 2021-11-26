using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Contracts.V1.Requests
{
    public class CreateCommentRequest
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
    }
}
