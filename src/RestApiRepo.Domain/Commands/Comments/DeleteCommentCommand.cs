using MediatR;
using RestApiRepo.Domain.Responses.V1.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Commands.Comments
{
    public class DeleteCommentCommand : IRequest<DeleteCommentResponse>
    {
        public Guid Id { get; set; }
        public string RequestingUserId { get; set; }
    }
}
