using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Requests.V1;
using RestApiRepo.Domain.Requests.V1.Posts;
using RestApiRepo.Domain.Responses.V1;
using RestApiRepo.Domain.Services;
using RestApiRepo.Extensions;
using RestApiRepo.Filters;
using RestApiRepo.PresentationServices;
using RestApiRepo.ResponseModels;
using RestApiRepo.Routes.V1.ApiRoutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.Controllers.V1
{
    [Authorize]
    [Route(ApiRoutes.Posts.PostBase)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        private readonly IUriBuilderFactory _uriBuilderFactory;


        public PostsController(IPostService postService,
            ITagService tagService,
            IMapper mapper,
            IPaginationService paginationService,
            IUriBuilderFactory uriBuilderFactory)
        {
            _postService = postService;
            _tagService = tagService;
            _mapper = mapper;
            _paginationService = paginationService;
            _uriBuilderFactory = uriBuilderFactory;
        }

        /// <summary>
        /// Get a Post by Id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            var response = _mapper.Map<PostResponse>(post);
            return Ok(new Response<PostResponse>(response));
        }
        /// <summary>
        /// Get All posts
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        [Cache(10)]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllPostsUserFilter filterQuery, [FromQuery] PaginationQuery query)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(query);
            var userFilter = _mapper.Map<UserFilter>(filterQuery);

            var posts = await _postService.GetPostsAsync(userFilter, paginationFilter);
            var response = _mapper.Map<List<PostResponse>>(posts);
            var paginationResponse = _paginationService.CreatePaginatedResponse(paginationFilter, response);
            return Ok(paginationResponse);
        }

        /// <summary>
        /// Update a post and optionally its tags, provided the post Id
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {

            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
                return BadRequest(new { errors = "you don't own this post" });

            var post = await _postService.GetPostByIdAsync(postId);
            var tags = request.NewTags
                 .Select(t => new Tag
                 {
                     UserCreatorId = HttpContext.GetUserId(),
                     TagName = t,
                     CreatedAt = DateTime.UtcNow,
                 });

            var existingTags = await _tagService.GetTagsByIdAsync(request.ExistingTags);
            var postTags = tags.Concat(existingTags).ToList();

            post.Name = request.Name;
            post.PostTags.Concat(postTags.Select(t => new PostTag
            {
                Post = post,
                Tag = t,
            }).ToList());

            var updated = await _postService.UpdatePostAsync(post);

            if (updated)
                return Ok();
            else
                return NotFound();
        }
        /// <summary>
        /// Delete a post provided its Id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
                return BadRequest(new { errors = "you don't own this post" });

            var deleted = await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();
            else
                return NotFound();
        }
        /// <summary>
        /// Create a post with optional tags
        /// </summary>
        /// <param name="postRequest"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> CreateAsync(PostRequest postRequest)
        {
            var tags = postRequest.NewTags
                .Select(t => new Tag
                {
                    UserCreatorId = HttpContext.GetUserId(),
                    TagName = t,
                    CreatedAt = DateTime.UtcNow
                }).ToList();
            var existingTags = await _tagService.GetTagsByIdAsync(postRequest.ExistingTags);
            var postTags = tags.Concat(existingTags).ToList();
            var post = new Post
            {
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId(),
            };
            post.PostTags = postTags.Select(t => new PostTag
            {
                Post = post,
                Tag = t,
            }).ToList();

            await _postService.CreatePostAsync(post);
            var response = _mapper.Map<PostResponse>(post);
            var uriBuilder = _uriBuilderFactory.CreateBuilder<PostResponse>();
            var request = HttpContext.Request;
            var baseUri = $"{request.Scheme}://{request.Host.ToUriComponent()}";
            var location = uriBuilder.GetRecordByIdUrl(baseUri, post.Id.ToString());

            return Created(location, new Response<PostResponse>(response));
        }
    }
}
