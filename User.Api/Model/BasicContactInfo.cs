using Newtonsoft.Json;

namespace User.Api.Model
{
    public class BasicContactInfo
    {
        [JsonProperty, JsonRequired] public string Name { get; set; }
        [JsonProperty, JsonRequired] public string PhoneNumber { get; set; }
        [JsonProperty, JsonRequired] public bool IsDirectContact { get; set; }
    }
}
