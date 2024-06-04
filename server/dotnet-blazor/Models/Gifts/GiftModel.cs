using System.Text.Json.Serialization;

namespace Blackbaud.PaymentsAPITutorial.Models
{
    public class GiftModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public string ConstituentType { get; set; }

        [JsonPropertyName("lookup_id")]
        public string LookupId { get; set; }

        [JsonPropertyName("first")]
        public string First { get; set; }

        [JsonPropertyName("last")]
        public string Last { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
