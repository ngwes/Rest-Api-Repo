using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Contracts.V1.Responses
{
    public class GetPostCommentsResponse
    {
        public IEnumerable<PostCommentResponse> Comments { get; set; }
    }
}
