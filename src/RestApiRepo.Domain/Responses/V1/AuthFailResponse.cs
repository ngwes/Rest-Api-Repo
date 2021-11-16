using System.Collections.Generic;

namespace RestApiRepo.Domain.Responses.V1 
{
    public class AuthFailResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
