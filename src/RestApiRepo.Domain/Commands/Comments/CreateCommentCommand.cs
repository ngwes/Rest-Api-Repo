using MediatR;
using RestApiRepo.Domain.Responses.V1.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Commands.Comments
{
    public class CreateCommentCommand : IRequest<CreateCommentResponse>
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
    }
}
