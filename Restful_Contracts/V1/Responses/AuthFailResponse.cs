using System.Collections.Generic;

namespace Rest_Api_Repo.Contracts.V1.Responses
{
    public class AuthFailResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
