using System.ComponentModel.DataAnnotations;

namespace Rest_Api_Repo.Domain.Requests.V1
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
