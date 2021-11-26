using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiRepo.Domain.Commands.Comments;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Queries.Comments;
using RestApiRepo.Domain.Requests.V1;
using RestApiRepo.Domain.Requests.V1.Comments;
using RestApiRepo.Domain.Requests.V1.Posts;
using RestApiRepo.Domain.Responses.V1.Comments;
using RestApiRepo.Extensions;
using RestApiRepo.PresentationServices;
using RestApiRepo.ResponseModels;
using RestApiRepo.Routes.V1.ApiRoutes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Controllers.V1
{
    [ApiController]
    [Route(ApiRoutes.Comments.CommentsBase)]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        private readonly IUriBuilderFactory _uriBuilderFactory;

        public CommentsController(IMediator mediator,
            IMapper mapper,
            IPaginationService paginationService,
            IUriBuilderFactory uriBuilderFactory)
        {
            _mediator = mediator;
            _mapper = mapper;
            _paginationService = paginationService;
            _uriBuilderFactory = uriBuilderFactory;
        }

        [HttpGet(ApiRoutes.Comments.Get)]
        public async Task<IActionResult> GetById(Guid commentId)
        {
            var getByIdQuery = new GetCommentByIdQuery { Id = commentId };
            var result = await _mediator.Send(getByIdQuery);
            if (result is null)
                return BadRequest();
            return Ok(new Response<CommentResponse>(result));
        }

        [HttpPost(ApiRoutes.Comments.Create)]
        public async Task<IActionResult> Create(CreateCommentRequest request)
        {
            var userId = HttpContext.GetUserId();
            var createCommand = new CreateCommentCommand { UserId = userId, Content = request.Content, PostId = request.PostId };
            var result = await _mediator.Send(createCommand);
            if (result is null || !result.Success)
                return BadRequest();
            var uriBuilder = _uriBuilderFactory.CreateBuilder<CommentResponse>();
            var baseUri = HttpContext.GetBaseUri();
            var location = uriBuilder.GetRecordByIdUrl(baseUri, result.CommentResponse.Id.ToString());
            return Created(location, new Response<CreateCommentResponse>(result));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(ApiRoutes.Comments.GetAll)]
        public async Task<IActionResult> GetAllComments([FromQuery] GetAllCommentsUserFilterQuery filterQuery, [FromQuery] PaginationQuery query)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(query);
            var userFilter = _mapper.Map<UserFilter>(filterQuery);
            var getAllComments = new GetAllCommentsQuery
            {
                PaginationFilter = paginationFilter,
                UserFilter = userFilter
            };
            var result = await _mediator.Send(getAllComments);
            if (result is null)
                return BadRequest();
            var response = _paginationService.CreatePaginatedResponse(paginationFilter, result.Comments.ToList());
            return Ok(response);
        }

        [HttpPut(ApiRoutes.Comments.Update)]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid commentId, [FromBody] UpdateCommentRequest request)
        {
            var updateCommentCommand = new UpdateCommentCommand
            {
                Id = commentId,
                NewContent = request.NewContent,
                RequestingUserId = HttpContext.GetUserId()
            };
            var result = await _mediator.Send(updateCommentCommand);
            if (result is null || !result.Success)
                return BadRequest();
            var uriBuilder = _uriBuilderFactory.CreateBuilder<CommentResponse>();
            var baseUri = HttpContext.GetBaseUri();
            var location = uriBuilder.GetRecordByIdUrl(baseUri, result.CommentResponse.Id.ToString());
            return Created(location, new Response<UpdateCommentResponse>(result));
        }

        [HttpDelete(ApiRoutes.Comments.Delete)]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
        {
            var updateCommentCommand = new DeleteCommentCommand
            {
                Id = commentId,
                RequestingUserId = HttpContext.GetUserId()
            };
            var result = await _mediator.Send(updateCommentCommand);
            if (result is null || !result.Success)
                return BadRequest();
            return Ok(new Response<DeleteCommentResponse>(result));
        }

        [HttpGet(ApiRoutes.Comments.GetPostComments)]
        public async Task<IActionResult> GetPostComments([FromRoute] Guid postId, [FromQuery] GetAllPostsUserFilter filterQuery, [FromQuery] PaginationQuery query)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(query);
            var userFilter = _mapper.Map<UserFilter>(filterQuery);
            var getPostComments = new GetPostCommentsQuery
            {
                PaginationFilter = paginationFilter,
                UserFilter = userFilter, 
                PostId = postId
            };
            var result = await _mediator.Send(getPostComments);
            if (result is null)
                return BadRequest();
            var response = _paginationService.CreatePaginatedResponse(paginationFilter, result.Comments.ToList());
            response.NextPage =  response.NextPage?.Replace("{postId}", postId.ToString());
            response.PreviousPage = response.PreviousPage?.Replace("{postId}", postId.ToString());
            return Ok(response);
        }

    }
}
