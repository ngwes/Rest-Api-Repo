using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiRepo.Domain.Responses.V1;
using RestApiRepo.Domain.Services;
using RestApiRepo.Filters;
using RestApiRepo.Routes.V1.ApiRoutes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApiRepo.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Authorize(Roles = "Admin, Poster")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<TagsResponse>>(await _tagService.GetTagsAsync()));
        }
        [HttpGet(ApiRoutes.Tags.GetAllWitApiKey)]
        [ApiKeyAuth]
        public async Task<IActionResult> GetAllWithApiKey()
        {
            return Ok(_mapper.Map<List<TagsResponse>>(await _tagService.GetTagsAsync()));
        }
        [HttpGet(ApiRoutes.Tags.GetAllWithPolicy)]
        [Authorize(Policy = "WorksForGoogle")]
        public async Task<IActionResult> GetAllWithPolicy()
        {
            return Ok(_mapper.Map<List<TagsResponse>>(await _tagService.GetTagsAsync()));
        }
    }
}
