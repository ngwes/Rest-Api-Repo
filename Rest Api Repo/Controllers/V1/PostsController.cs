using Microsoft.AspNetCore.Mvc;
using Rest_Api_Repo.Contracts;
using Rest_Api_Repo.Contracts.V1;
using Rest_Api_Repo.Contracts.V1.Requests;
using Rest_Api_Repo.Contracts.V1.Responses;
using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Controllers.V1
{
    public class PostsController : Controller
    {
        private List<Post> _posts;

        public PostsController()
        {
            _posts = new List<Post>();
            for (int i = 0; i < 5; i++)
            {
                _posts.Add(new Post { Id = Guid.NewGuid().ToString() });
            }
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_posts);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create(PostRequest postRequest)
        {
            var post = new Post { Id = postRequest.Id };
            if (string.IsNullOrEmpty(post.Id))
                post.Id = Guid.NewGuid().ToString();
            _posts.Add(post);
            var response = new PostResponse { Id = post.Id };
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var location = $"{baseUrl}/{ApiRoutes.Posts.Get.Replace("{postId}", post.Id)}";
            return Created(location, response);
        }
    }
}
