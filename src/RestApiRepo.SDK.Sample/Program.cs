using Refit;
using RestApiRepo.Contracts.V1.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApiRepo.SDK.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var apiUrl = "http://localhost:64115/";
            //var apiUrl = "https://localhost:10000/";
            var cachedToken = string.Empty;

            var identityApi = RestService.For<IIdentityApi>(apiUrl);
            var tagApi = RestService.For<ITagApi>(apiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            var postApi = RestService.For<IPostApi>(apiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            var commentApi = RestService.For<ICommentApi>(apiUrl, new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });
            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "sdkAccount@gmail.com",
                Password = "Pa$$word1"
            });

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest
            {
                Email = "sdkAccount@gmail.com",
                Password = "Pa$$word1"
            });

            var refreshResponse = await identityApi.RefreshAsync(new RefreshTokenRequest
            {
                Token = loginResponse.Content.Token,
                RefreshToken = loginResponse.Content.RefreshToken
            });

            cachedToken = loginResponse.Content.Token;

            var createPostResponse = await postApi.CreatePostAsync(new PostRequest
            {
                Name = "Create",
                NewTags = new List<string> { $"#{Guid.NewGuid()}" },
                ExistingTags = new List<Guid>()
            });
            var getPostResponse = await postApi.GetPostAsync(createPostResponse.Content.Data.Id);
            var updatePostResponse = await postApi.UpdatePostAsync(createPostResponse.Content.Data.Id
                , new UpdatePostRequest
                {
                    Name = "Update",
                    NewTags = new List<string> { "#sampleUpdateTag" },
                    ExistingTags = new List<Guid>()
                });
            var getAllPostResponse = await postApi.GetAllPostsAsync(pageNumber: 1, pageSize: 1);
            var getAllTags = await tagApi.GetAllTagsAsync();
            var getAllTagsWithApiKey = await tagApi.GetAllTagsWithApiKeyAsync("MySecretKey");
            var getAllTagsWithPolicy = await tagApi.GetAllTagsWithPolicyAsync();

            var createCommentResponse = await commentApi.CreateCommentAsync(new CreateCommentRequest
            {
                Content = "SdkContent",
                PostId = getPostResponse.Content.Data.Id
            });
            var getCommentResponse = await commentApi.GetCommentByIdAsync(createCommentResponse.Content.Data.CommentResponse.Id);
            var updateCommentResponse = await commentApi.UpdateCommentByIdAsync(createCommentResponse.Content.Data.CommentResponse.Id
                , new UpdateCommentRequest
                {
                    NewContent = "NewSdkContent"
                });
            var getAllCommentResponse = await commentApi.GetAllCommentsAsync(pageNumber: 1, pageSize: 1);
            var getAllPostCommentResponse = await commentApi.GetPostComments(getPostResponse.Content.Data.Id,pageNumber: 1, pageSize: 1);
            var deletePostResponse = await postApi.DeletePostAsync(getPostResponse.Content.Data.Id);
        }
    }
}
