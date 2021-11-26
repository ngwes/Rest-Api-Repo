using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Contracts.V1.Responses
{
    public class PostCommentResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
