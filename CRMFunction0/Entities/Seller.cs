using Newtonsoft.Json;

namespace CRMFunction0.Entities
{
    public class Seller
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "";
        [JsonProperty("phone")]
        public string Phone { get; set; } = "";
        [JsonProperty("email")]
        public string Email { get; set; } = "";
    }
}