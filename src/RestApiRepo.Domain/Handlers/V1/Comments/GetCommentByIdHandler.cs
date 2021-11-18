using AutoMapper;
using MediatR;
using RestApiRepo.Domain.Queries.Comments;
using RestApiRepo.Domain.Responses.V1.Comments;
using RestApiRepo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Handlers.V1.Comments
{
    public class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, CommentResponse>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public GetCommentByIdHandler(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        public async Task<CommentResponse> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await  _commentService.GetCommentByIdAsync(request.Id);
            //TODO: use automapper
            return new CommentResponse { 
                Content = comment.Content,
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId
            };
        }
    }
}
