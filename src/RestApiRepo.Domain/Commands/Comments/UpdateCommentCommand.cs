using MediatR;
using RestApiRepo.Domain.Responses.V1.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Commands.Comments
{
    public class UpdateCommentCommand : IRequest<UpdateCommentResponse>
    {
        public Guid Id { get; set; }
        public string RequestingUserId { get; set; }
        public string NewContent { get; set; }
    }
}
