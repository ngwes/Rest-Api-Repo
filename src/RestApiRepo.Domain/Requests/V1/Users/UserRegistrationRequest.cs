using System.ComponentModel.DataAnnotations;

namespace RestApiRepo.Domain.Requests.V1.Users
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
