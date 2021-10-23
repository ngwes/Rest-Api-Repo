using FluentAssertions;
using Newtonsoft.Json;
using Rest_Api_Repo;
using Rest_Api_Repo.Contracts.V1;
using Rest_Api_Repo.Contracts.V1.Requests;
using Rest_Api_Repo.Contracts.V1.Responses;
using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Restfull_IntegrationTest
{
    public class PostControllerTest : IClassFixture<InMemoryApplicationFactory<Startup>>
    {
        private readonly InMemoryApplicationFactory<Startup> _factory;

        public PostControllerTest(InMemoryApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            //Arrange
            var client = await _factory.CreateClient().AuthenticateAsync();
            var url = $"{ApiRoutes.Posts.PostBase}/{ApiRoutes.Posts.GetAll}";
            _factory.ClearTable<Post>();
            //Act
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<List<PostResponse>>(responseContent);
            //Assert
            responseEntity.Should().BeEmpty(); 
        }


        [Fact]
        public async Task Get_ReturnPost_WhenPostExistsInTheDatabase()
        {

            //Arrange
            var client = await _factory.CreateClient().AuthenticateAsync();
            var response = await client.CreatePostAsync(new PostRequest { Name = Guid.NewGuid().ToString(), NewTags = new List<string> { Guid.NewGuid().ToString() } });
            var url = $"{ApiRoutes.Posts.PostBase}/{ApiRoutes.Posts.Get}";
            //Act
            var PostResponse = await client.GetAsync(url.Replace("{postId}",response.Id.ToString()));
            PostResponse.EnsureSuccessStatusCode();
            var responseContent = await PostResponse.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<PostResponse>(responseContent);

            //Assert
            responseEntity.Should().NotBeNull();
            responseEntity.Id.Should().Be(response.Id);
            responseEntity.Name.Should().Be(response.Name);

        }

        [Fact]
        public async Task Get_ReturnPostWithTags_WhenPostWithTagsExistsInTheDatabase()
        {

            //Arrange
            var client = await _factory.CreateClient().AuthenticateAsync();
            var response = await client.CreatePostAsync(new PostRequest { Name = Guid.NewGuid().ToString(), NewTags = new List<string> { Guid.NewGuid().ToString() } });
            var url = $"{ApiRoutes.Posts.PostBase}/{ApiRoutes.Posts.Get}";
            //Act
            var PostResponse = await client.GetAsync(url.Replace("{postId}", response.Id.ToString()));
            PostResponse.EnsureSuccessStatusCode();
            var responseContent = await PostResponse.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<PostResponse>(responseContent);

            //Assert
            responseEntity.Tags.Should().NotBeNull();
            responseEntity.Tags.Should().NotBeEmpty();


        }

    }
}
