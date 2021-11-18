using AutoMapper;
using MediatR;
using RestApiRepo.Domain.Queries.Comments;
using RestApiRepo.Domain.Responses.V1.Comments;
using RestApiRepo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Handlers.V1.Comments
{
    public class GetPostCommentsHandler : IRequestHandler<GetPostCommentsQuery, GetPostCommentsResponse>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public GetPostCommentsHandler(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        public async Task<GetPostCommentsResponse> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentService.GetPostCommentsAsync(request.PostId, request.UserFilter, request.PaginationFilter);
            //TODO: mapping with mapper
            return new GetPostCommentsResponse {
                Comments = comments.Select(c => new PostCommentResponse { 
                   Content = c.Content,
                   Id = c.Id,
                   PostId = c.PostId,
                   UserId = c.UserId,
                   CreateAt = c.CreateAt
                })
            };
        }
    }
}
