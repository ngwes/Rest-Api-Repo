using System;

namespace RestApiRepo.Domain.Responses.V1
{
    public class TagsResponse
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
    }
}