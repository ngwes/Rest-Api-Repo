using System;

namespace RestApiRepo.Contracts.V1.Responses
{
    public class TagsResponse
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }
}