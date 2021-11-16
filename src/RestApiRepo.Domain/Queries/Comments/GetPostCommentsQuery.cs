using MediatR;
using RestApiRepo.Domain.Responses.V1.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Queries.Comments
{
    public class GetPostCommentsQuery : IRequest<GetPostCommentsResponse>
    {
        public Guid PostId { get; set; }
    }
}
