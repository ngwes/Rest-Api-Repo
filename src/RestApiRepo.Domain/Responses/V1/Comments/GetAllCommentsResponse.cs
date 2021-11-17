using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Responses.V1.Comments
{
    public class GetAllCommentsResponse
    {
        public IEnumerable<CommentResponse> Comments { get; set; }
    }
}
