using Refit;
using RestApiRepo.Contracts.V1.Requests;
using RestApiRepo.Contracts.V1.Responses;
using RestApiRepo.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.SDK
{
    [Headers("Authorization: Bearer")]
    public interface IPostApi
    {
        [Get("/api/v1/posts")]
        Task<ApiResponse<PagedResponse<PostResponse>>> GetAllPostsAsync([AliasAs("pageSize")]int pageSize, [AliasAs("pageNumber")]int pageNumber, [AliasAs("userId")] string userId = null);

        [Get("/api/v1/posts/{postId}")]
        Task<ApiResponse<Response<PostResponse>>> GetPostAsync(Guid postId);

        [Post("/api/v1/posts")]
        Task<ApiResponse<Response<PostResponse>>> CreatePostAsync([Body]PostRequest request);

        [Put("/api/v1/posts/{postId}")]
        Task<ApiResponse<string>> UpdatePostAsync(Guid postId, [Body] UpdatePostRequest request);

        [Delete("/api/v1/posts/{postId}")]
        Task<ApiResponse<string>> DeletePostAsync(Guid postId);

    }
}
