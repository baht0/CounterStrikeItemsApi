using System.Text.Json.Serialization;

namespace WebAdminPanel.Models.DTOs
{
    public class ApiStringResponse
    {
        [JsonExtensionData]
        public Dictionary<string, object>? DynamicValues { get; set; }

        public string? GetFirstValue() => DynamicValues?.FirstOrDefault().Value.ToString();
    }
}
