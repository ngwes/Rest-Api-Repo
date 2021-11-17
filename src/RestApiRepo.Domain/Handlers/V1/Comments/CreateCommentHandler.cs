using AutoMapper;
using MediatR;
using RestApiRepo.Domain.Commands.Comments;
using RestApiRepo.Domain.Entitites;
using RestApiRepo.Domain.Responses.V1.Comments;
using RestApiRepo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Handlers.V1.Comments
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CreateCommentResponse>
    {
        private readonly ICommentService _commentService;

        public CreateCommentHandler(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<CreateCommentResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                Content = request.Content,
                CreateAt = DateTime.Now,
                UserId = request.UserId,
                PostId = request.PostId
            };
            var result = await _commentService.CreateCommentAsync(comment);
            var response = new CommentResponse {
                Content = comment.Content,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Id = comment.Id
            };
            return new CreateCommentResponse { Success = result, CommentResponse = response };
        }
    }
}
