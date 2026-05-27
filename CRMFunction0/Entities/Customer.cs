using Newtonsoft.Json;

namespace CRMFunction0.Entities
{
    public class Customer
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "";

        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("phone")]
        public string Phone { get; set; } = "";

        [JsonProperty("email")]
        public string Email { get; set; } = "";

        [JsonProperty("adress")]
        public string Address { get; set; } = "";

        [JsonProperty("seller")]
        public Seller Seller { get; set; } = new();



    }
}