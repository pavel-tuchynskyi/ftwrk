using Newtonsoft.Json;

namespace FTWRK.Infrastructure.Idenity.Models
{
    public class ExternalAuthUserResult
    {
        [JsonProperty("name")]
        public string UserName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }

        public ExternalAuthUserResult(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }
    }
}
