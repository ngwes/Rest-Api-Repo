using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rest_Api_Repo.ResponseModels;
using Rest_Api_Repo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Rest_Api_Repo.Routes.V1.ApiRoutes;
using Rest_Api_Repo.Domain.Responses.V1;

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
