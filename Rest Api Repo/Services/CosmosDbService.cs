using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Services
{
    public class CosmosDbService<T> : ICosmosDbService<T>

    {
        private Container _container;

        public CosmosDbService(CosmosClient cosmosDbClient,
            string databaseName, 
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        public async Task<bool> AddAsync(T item, string itemId)
        {
            var result = await _container.CreateItemAsync(item, new PartitionKey(itemId));
            return result.StatusCode  == HttpStatusCode.OK;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<T> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) 
            {
                return default(T);
            }
        }

        public async Task<IEnumerable<T>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<bool> UpdateAsync(string id, T item)
        {
            var result = await _container.UpsertItemAsync(item, new PartitionKey(id));
            return result.StatusCode == HttpStatusCode.OK;
        }
    }
}
