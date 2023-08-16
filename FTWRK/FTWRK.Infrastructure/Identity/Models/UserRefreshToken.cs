namespace FTWRK.Infrastructure.Idenity.Models
{
    public class UserRefreshToken
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
