using System.Collections.Generic;

namespace Rest_Api_Repo.Domain.Responses.V1 
{
    public class AuthFailResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
