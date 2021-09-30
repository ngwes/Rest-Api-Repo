using Rest_Api_Repo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Services
{
    public class CosmosPostService : IPostService
    {
        private readonly ICosmosDbService<Post> _cosmosDbService;

        public CosmosPostService(ICosmosDbService<Post> cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            return await _cosmosDbService.AddAsync(post, post.Id.ToString());
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            return await _cosmosDbService.DeleteAsync(postId.ToString());
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            return await _cosmosDbService.GetAsync(id.ToString());
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return (await _cosmosDbService.GetMultipleAsync("SELECT * FROM c")).ToList();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            return await _cosmosDbService.UpdateAsync(postToUpdate.Id.ToString(), postToUpdate);
        }
    }
}
