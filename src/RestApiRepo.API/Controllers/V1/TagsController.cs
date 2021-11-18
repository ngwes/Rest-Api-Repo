using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiRepo.ResponseModels;
using RestApiRepo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RestApiRepo.Routes.V1.ApiRoutes;
using RestApiRepo.Domain.Responses.V1;
using RestApiRepo.Filters;

namespace RestApiRepo.Controllers.V1
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
        [ApiKeyAuth]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<TagsResponse>>(await _tagService.GetTagsAsync()));
        }
    }
}
