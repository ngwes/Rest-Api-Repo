using Newtonsoft.Json;
using RestApiRepo.Domain.Requests.V1;
using RestApiRepo.Domain.Requests.V1.Posts;
using RestApiRepo.Domain.Requests.V1.Users;
using RestApiRepo.Domain.Responses.V1;
using RestApiRepo.ResponseModels;
using RestApiRepo.Routes.V1.ApiRoutes;
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
                .DeserializeObject<Response<PostResponse>>(responseContent);
            return responseEntity.Data;
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
                .SerializeObject(new UserLoginRequest
                {
                    Email = "test@integration.com",
                    Password = "Pa$$word1"
                }),
                Encoding.UTF8,
                "application/json");

            var authenticationUrl = $"{ApiRoutes.Identity.IdentityBase}/{ApiRoutes.Identity.Login}";
            var response = await client.PostAsync(authenticationUrl, requestContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert
                .DeserializeObject<AuthSuccessResponse>(responseContent);

            return responseEntity.Token;
        }
    }
}
