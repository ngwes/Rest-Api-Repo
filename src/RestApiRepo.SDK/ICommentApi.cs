using Refit;
using RestApiRepo.Contracts.V1.Requests;
using RestApiRepo.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.SDK
{
    [Headers("Authorization: Bearer")]
    public interface ICommentApi
    {
        [Get("/api/v1/comments")]
        Task<ApiResponse<PagedResponse<CommentResponse>>> GetAllCommentsAsync([AliasAs("pageSize")] int pageSize, [AliasAs("pageNumber")] int pageNumber, [AliasAs("userId")] string userId = null);

        [Get("/api/v1/comments/{commentId}")]
        Task<ApiResponse<Response<CommentResponse>>> GetCommentByIdAsync(Guid commentId);

        [Get("/api/v1/comments/postComments/{postId}")]
        Task<ApiResponse<PagedResponse<CommentResponse>>> GetPostComments(Guid postId, [AliasAs("pageSize")] int pageSize, [AliasAs("pageNumber")] int pageNumber, [AliasAs("userId")] string userId = null);

        [Post("/api/v1/comments")]
        Task<ApiResponse<Response<CreateCommentResponse>>> CreateCommentAsync([Body] CreateCommentRequest request);

        [Put("/api/v1/comments/{commentId}")]
        Task<ApiResponse<Response<UpdateCommentResponse>>> UpdateCommentByIdAsync(Guid commentId, [Body] UpdateCommentRequest request);

        [Delete("/api/v1/comments/{commentId}")]
        Task<ApiResponse<Response<DeleteCommentResponse>>> DeleteCommentAsync(Guid commentId);
    }
}
