using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Requests.V1;
using RestApiRepo.Domain.Requests.V1.Posts;
using RestApiRepo.Extensions;
using RestApiRepo.Routes.V1.ApiRoutes;
using RestApiRepo.Domain.Commands.Comments;
using RestApiRepo.Domain.Queries.Comments;
using RestApiRepo.Domain.Requests.V1.Comments;
using System;
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

        public CommentsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Comments.Get)]
        public async Task<IActionResult> GetById(Guid commentId)
        {
            var getByIdQuery = new GetCommentByIdQuery { Id = commentId };
            var result = await _mediator.Send(getByIdQuery);
            if (result is null)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet(ApiRoutes.Comments.Create)]
        public async Task<IActionResult> Create(CreateCommentRequest request)
        {
            var userId = HttpContext.GetUserId();
            var createCommand = new CreateCommentCommand { UserId = userId, Content = request.Content, };
            var result = await _mediator.Send(createCommand);
            if (result.Success)
                return Ok();
            return BadRequest();
        }

        [Authorize("Admin")]
        [HttpGet(ApiRoutes.Comments.GetAll)]
        public async Task<IActionResult> GetAllComments([FromQuery] GetAllPostsQuery filterQuery, [FromQuery] PaginationQuery query)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(query);
            var userFilter = _mapper.Map<UserFilter>(filterQuery);
            var getAllComments = new GetAllCommentsQuery
            {
                paginationFilter = paginationFilter,
                userFilter = userFilter
            };
            var result = await _mediator.Send(getAllComments);
            if (result is null)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet(ApiRoutes.Comments.Update)]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid commentId, [FromBody] UpdateCommentRequest request)
        {
            var updateCommentCommand = new UpdateCommentCommand
            {
                Id = commentId,
                NewContent = request.NewContent,
                RequestingUserId = HttpContext.GetUserId()
            };
            var result = await _mediator.Send(updateCommentCommand);
            if (result is null)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet(ApiRoutes.Comments.Delete)]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
        {
            var updateCommentCommand = new DeleteCommentCommand
            {
                Id = commentId,
                RequestingUserId = HttpContext.GetUserId()
            };
            var result = await _mediator.Send(updateCommentCommand);
            if (result is null)
                return BadRequest();
            return Ok(result);
        }

    }
}
