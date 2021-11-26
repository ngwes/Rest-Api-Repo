using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Contracts.V1.Requests
{
    public class UpdateCommentRequest
    {
        public Guid Id { get; set; }
        public string NewContent { get; set; }
    }
}
