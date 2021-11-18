using MediatR;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Responses.V1.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Queries.Comments
{
    public class GetAllCommentsQuery : IRequest<GetAllCommentsResponse>
    {
        public PaginationFilter PaginationFilter { get; set; }
        public UserFilter UserFilter { get; set; }
    }
}
