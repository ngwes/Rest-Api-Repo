using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rest_Api_Repo.Contracts.V1;
using Rest_Api_Repo.Contracts.V1.Responses;
using Rest_Api_Repo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(ApiRoutes.Tags.TagsBase)]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }


        [HttpGet(ApiRoutes.Tags.GetAll)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = (await _tagService.GetTagsAsync())
                .Select(t => new TagsResponse {
                    Id = t.Id,
                    TagName = t.TagName
                });
            return Ok(response);
        }
    }
}
