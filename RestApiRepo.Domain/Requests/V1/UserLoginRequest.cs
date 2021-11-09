namespace Rest_Api_Repo.Domain.Requests.V1
{
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
