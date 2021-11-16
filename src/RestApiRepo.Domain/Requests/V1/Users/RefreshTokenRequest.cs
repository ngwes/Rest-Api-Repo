namespace RestApiRepo.Domain.Requests.V1.Users
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
