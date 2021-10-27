using Refit;
using Rest_Api_Repo.Contracts.V1.Requests;
using Restful_SDK;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestFul_SDK_Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var apiUrl = "http://localhost:58465/";
            var cachedToken = string.Empty;

            var identityApi = RestService.For<IIdentityApi>(apiUrl);
            var postApi = RestService.For<IPostApi>(apiUrl, new RefitSettings { 
                AuthorizationHeaderValueGetter = ()=> Task.FromResult(cachedToken)
            });

            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest
            {
                Email = "sdkAccount@gmail.com",
                Password = "Pa$$word1"
            });

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest { 
                Email = "sdkAccount@gmail.com",
                Password = "Pa$$word1"
            });

            
            var refreshResponse = await identityApi.RefreshAsync(new RefreshTokenRequest { 
                Token = loginResponse.Content.Token,
                RefreshToken = loginResponse.Content.RefreshToken
            });

            cachedToken = loginResponse.Content.Token;

            var createPostResponse = await postApi.CreatePostAsync(new PostRequest {
                Name = "Create",
                NewTags = new List<string> { "#sampleCreateTag" },
                ExistingTags = new List<Guid>()
            });
            var getPostResponse = await postApi.GetPostAsync(createPostResponse.Content.Id);
            var updatePostResponse = await postApi.UpdatePostAsync(createPostResponse.Content.Id
                , new UpdatePostRequest { 
                    Name ="Update",
                    NewTags = new List<string> { "#sampleUpdateTag" },
                    ExistingTags = new List<Guid>()
                });
            var getAllPostResponse = await postApi.GetAllPostsAsync(pageNumber:1,pageSize:1);
            var deletePostResponse = await postApi.DeletePostAsync(getPostResponse.Content.Id);

        }
    }
}
