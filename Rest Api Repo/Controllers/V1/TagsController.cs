using AutoMapper;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [Route(ApiRoutes.Tags.TagsBase)]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagsController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }


        [HttpGet(ApiRoutes.Tags.GetAll)]
        [Authorize(Policy = "WorksForGoogle")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<TagsResponse>>(await _tagService.GetTagsAsync()));
        }
    }
}
