using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest_Api_Repo.Domain;
using Rest_Api_Repo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Installers
{
    public class CosmosInstaller /*: IInstaller*/
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<ICosmosDbService<Post>>(InitializeCosmosClientInstanceAsync(configuration.GetSection("CosmosSettings")).GetAwaiter().GetResult());
            //services.AddSingleton<IPostService, CosmosPostService>();
        //var cosmosStoreSettings = new CosmosStoreSettings(
        //        configuration["CosmosSettings:DatabaseName"],
        //        configuration["CosmosSettings:AccountUri"],
        //        configuration["CosmosSettings:AccountKey"],
        //        new ConnectionPolicy
        //        {
        //            ConnectionMode = ConnectionMode.Direct,
        //            ConnectionProtocol = Protocol.Tcp
        //        });

        //    //It only register the cosmos store
        //    services.AddCosmosStore<CosmosPostDto>(cosmosStoreSettings);
        }

        private static async Task<CosmosDbService<Post>> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];
            var account = configurationSection["AccountUri"];
            var key = configurationSection["AccountKey"];
            var client = new CosmosClient(account, key);
            var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var cosmosDbService = new CosmosDbService<Post>(client, databaseName, containerName);
            return cosmosDbService;
        }
    }
}
