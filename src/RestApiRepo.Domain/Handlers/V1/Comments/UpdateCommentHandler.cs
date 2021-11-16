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
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, UpdateCommentResponse>
    {
        private readonly ICommentService _commentService;

        public UpdateCommentHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<UpdateCommentResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            if (await _commentService.UserOwnsComment(request.Id, request.RequestingUserId))
                return new UpdateCommentResponse { Success = false };

            var comment = await _commentService.GetCommentByIdAsync(request.Id);
            comment.Content = request.NewContent;
            var result = await _commentService.UpdateCommentAsync(comment);

            return new UpdateCommentResponse { Success = result };
        }
    }
}
