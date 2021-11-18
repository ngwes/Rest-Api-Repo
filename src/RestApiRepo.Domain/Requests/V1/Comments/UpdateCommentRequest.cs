using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Requests.V1.Comments
{
    public class UpdateCommentRequest
    {
        public Guid Id { get; set; }
        public string NewContent { get; set; }
    }
}
