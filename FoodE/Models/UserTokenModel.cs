using Newtonsoft.Json;

namespace FoodE.Drivers
{
    public class UserTokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

    }
}