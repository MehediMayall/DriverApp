using Newtonsoft.Json;

namespace UseCases.Dtos;

[JsonObject("Cors")]
public class CorsSettings
{
    [JsonProperty("AllowedOrigin")]
    public string AllowedOrigin { get; set; }
}
