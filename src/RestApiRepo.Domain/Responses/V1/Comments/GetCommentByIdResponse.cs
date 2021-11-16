using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Responses.V1.Comments
{
    public class GetCommentByIdResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
