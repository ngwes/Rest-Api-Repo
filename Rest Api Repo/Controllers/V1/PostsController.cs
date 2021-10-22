using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rest_Api_Repo.Contracts.V1;
using Rest_Api_Repo.Contracts.V1.Requests;
using Rest_Api_Repo.Contracts.V1.Responses;
using Rest_Api_Repo.Domain;
using Rest_Api_Repo.Extensions;
using Rest_Api_Repo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(ApiRoutes.Posts.PostBase)]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public PostsController(IPostService postService, ITagService tagService, IMapper mapper)
        {
            _postService = postService;
            _tagService = tagService;
            _mapper = mapper;
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
            var response =  _mapper.Map<PostResponse>(post);
            return Ok(response);
        }
        /// <summary>
        /// Get All posts
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAllAsync()
        {
            var posts = await _postService.GetPostsAsync();
            var response = _mapper.Map<List<PostResponse>>(posts);
            return Ok(response);
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
                     CreatedAt = DateTime.UtcNow
                 });
            var existingTags = _tagService.GetTagsById(request.ExistingTags);
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
        [Authorize(Roles ="Admin")]
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
            var tags =  postRequest.NewTags
                .Select(t => new Tag
                    {
                        UserCreatorId = HttpContext.GetUserId(),
                        TagName = t,
                        CreatedAt = DateTime.UtcNow
                });
            var existingTags = _tagService.GetTagsById(postRequest.ExistingTags);
            var postTags = tags.Concat(existingTags).ToList();
            var post = new Post
            {
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId(),
            };
            post.PostTags = postTags.Select(t => new PostTag {
                Post = post,
                Tag = t,
            }).ToList();

            await _postService.CreatePostAsync(post);
            var response = _mapper.Map<PostResponse>(post);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var location = $"{baseUrl}/{ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString())}";
            return Created(location, response);
        }
    }
}
