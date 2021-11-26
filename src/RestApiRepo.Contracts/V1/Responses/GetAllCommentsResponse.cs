using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Contracts.V1.Responses
{
    public class GetAllCommentsResponse
    {
        public IEnumerable<CommentResponse> Comments { get; set; }
    }
}
