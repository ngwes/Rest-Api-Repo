using AutoMapper;
using MediatR;
using RestApiRepo.Domain.Queries.Comments;
using RestApiRepo.Domain.Repositories;
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
    public class GetAllCommentsHandler : IRequestHandler<GetAllCommentsQuery, GetAllCommentsResponse>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public GetAllCommentsHandler(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        public async Task<GetAllCommentsResponse> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            var allComments = await _commentService.GetCommentsAsync(request.userFilter, request.paginationFilter);
            //TODO: var response = _mapper.Map<GetCommentByIdResponse>(allComments);
            var response = allComments.Select(c => new GetCommentByIdResponse
            {
                Content = c.Content,
                UserId = c.UserId,
                Id = c.Id,
                PostId = c.PostId
            });
            return new GetAllCommentsResponse { Comments = response };
        }
    }
}
