namespace Rest_Api_Repo.Domain.Requests.V1
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
