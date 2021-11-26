using Refit;
using RestApiRepo.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.SDK
{
    [Headers("Authorization: Bearer")]
    public interface ITagApi
    {
        [Get("/api/v1/tags")]
        Task<ApiResponse<List<TagsResponse>>> GetAllTagsAsync();

        [Get("/api/v1/tags/apiKey")]
        Task<ApiResponse<List<TagsResponse>>> GetAllTagsWithApiKeyAsync([Header("ApiKey")]string apiKey);

        [Get("/api/v1/tags/policy")]
        Task<ApiResponse<List<TagsResponse>>> GetAllTagsWithPolicyAsync();
    }
}
