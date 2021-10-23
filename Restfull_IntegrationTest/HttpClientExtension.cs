using Newtonsoft.Json;
using Rest_Api_Repo.Contracts.V1;
using Rest_Api_Repo.Contracts.V1.Requests;
using Rest_Api_Repo.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Restfull_IntegrationTest
{
    public static class HttpClientExtension
    {
        public async static Task<PostResponse> CreatePostAsync(this HttpClient client, PostRequest request)
        {
            var requestContent = new StringContent(JsonConvert
                .SerializeObject(request),
                Encoding.UTF8,
                "application/json");
            var url = $"{ApiRoutes.Posts.PostBase}/{ApiRoutes.Posts.Create}";
            var response = await client.PostAsync(url, requestContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert
                .DeserializeObject<PostResponse>(responseContent);
            return responseEntity;
        }

        public async static Task<HttpClient> AuthenticateAsync(this HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new
                AuthenticationHeaderValue("Bearer", await GetJwtAsync(client));
            return client;
        }
        private static async Task<string> GetJwtAsync(HttpClient client)
        {

            var requestContent = new StringContent(JsonConvert
                .SerializeObject(new UserRegistrationRequest
                {
                    Email = "test@integration.com",
                    Password = "Pa$$word1"
                }),
                Encoding.UTF8,
                "application/json");
            var registrationUrl = $"{ApiRoutes.Identity.IdentityBase}/{ApiRoutes.Identity.Register}";
            var response = await client.PostAsync(registrationUrl, requestContent);
            if(!response.IsSuccessStatusCode)
            {
                var authenticationUrl = $"{ApiRoutes.Identity.IdentityBase}/{ApiRoutes.Identity.Login}";
                response = await client.PostAsync(authenticationUrl, requestContent);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert
                .DeserializeObject<AuthSuccessResponse>(responseContent);

            return responseEntity.Token;
        }
    }
}
