using Newtonsoft.Json;

namespace Rest_Api_Repo.Domain
{
    //[CosmosCollection("posts")]
    public class CosmosPostDto
    {
        //[CosmosPartitionKey]
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
