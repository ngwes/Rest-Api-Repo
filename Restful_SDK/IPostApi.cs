using Refit;
using Rest_Api_Repo.Contracts.V1.Requests;
using Rest_Api_Repo.Contracts.V1.Responses;
using RestApi_Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Restful_SDK
{
    [Headers("Authorization: Bearer")]
    public interface IPostApi
    {
        [Get("/api/v1/posts")]
        Task<ApiResponse<PagedResponse<PostResponse>>> GetAllPostsAsync([AliasAs("pageSize")]int pageSize, [AliasAs("pageNumber")]int pageNumber, [AliasAs("userId")] string userId = null);

        [Get("/api/v1/posts/{postId}")]
        Task<ApiResponse<PostResponse>> GetPostAsync(Guid postId);

        [Post("/api/v1/posts")]
        Task<ApiResponse<PostResponse>> CreatePostAsync([Body]PostRequest request);

        [Put("/api/v1/posts/{postId}")]
        Task<ApiResponse<string>> UpdatePostAsync(Guid postId, [Body] UpdatePostRequest request);

        [Delete("/api/v1/posts/{postId}")]
        Task<ApiResponse<string>> DeletePostAsync(Guid postId);

    }
}
