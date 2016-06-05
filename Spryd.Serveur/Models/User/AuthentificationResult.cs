using Newtonsoft.Json;

namespace Spryd.Server.Models
{
    public class AuthenticationResult
    {
        public AuthenticationResult()
        {
            
        }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User User { get; set; }
    }
}