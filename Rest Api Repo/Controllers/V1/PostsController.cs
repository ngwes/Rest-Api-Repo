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

        public PostsController(IPostService postService, ITagService tagService)
        {
            _postService = postService;
            _tagService = tagService;
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            return Ok(new PostResponse { 
                Name = post.Name,
                UserId = post.UserId,
                Id = post.Id, 
                Tags = post.PostTags.Select(t => t.Tag.TagName).ToList() 
            });
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAllAsync()
        {

            return Ok((await _postService.GetPostsAsync())
                .Select(x=>new PostResponse {
                    Name = x.Name,
                    UserId = x.UserId,
                    Id = x.Id,
                    Tags = x.PostTags.Select(t=>t.Tag.TagName).ToList()
                }));
        }

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
            var response = new PostResponse { Id = post.Id , Name = post.Name, UserId = post.UserId};
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var location = $"{baseUrl}/{ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString())}";
            return Created(location, response);
        }
    }
}
