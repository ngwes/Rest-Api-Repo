using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Contracts.V1.Responses
{
    public class CreateCommentResponse
    {
        public bool Success { get; set; }
        public CommentResponse CommentResponse{ get; set; }
    }
}
