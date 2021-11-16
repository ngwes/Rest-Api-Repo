using AutoMapper;
using MediatR;
using RestApiRepo.Domain.Commands.Comments;
using RestApiRepo.Domain.Responses.V1.Comments;
using RestApiRepo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Handlers.V1.Comments
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentResponse>
    {
        private readonly ICommentService _commentService;

        public DeleteCommentHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<DeleteCommentResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            if (await _commentService.UserOwnsComment(request.Id, request.RequestingUserId))
                return new DeleteCommentResponse { Success = false };

            var result = await _commentService.DeleteCommentAsync(request.Id);
            return new DeleteCommentResponse { Success = result };
        }
    }
}
