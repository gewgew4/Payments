using System.Text.Json;

namespace Payments.Application.Options
{
    public class ExternalServiceOptions
    {
        public const string External = "External";
        public string ApiUrl { get; set; } = string.Empty;
        public JsonSerializerOptions? JsonSerializerOptions { get; set; } 
    }
}
