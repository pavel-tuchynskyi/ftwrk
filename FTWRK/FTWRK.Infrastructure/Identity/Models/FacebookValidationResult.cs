using Newtonsoft.Json;

namespace FTWRK.Infrastructure.Idenity.Models
{
    public class FacebookValidationResult
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
    }
}
