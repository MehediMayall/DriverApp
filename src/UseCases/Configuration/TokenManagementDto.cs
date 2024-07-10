using Newtonsoft.Json;

namespace UseCases.Dtos;

[JsonObject("tokenManagement")]
public class TokenManagementDto
{
    [JsonProperty("secret")]
    public string Secret { get; set; }

    [JsonProperty("issuer")]
    public string Issuer { get; set; }

    [JsonProperty("audience")]
    public string Audience { get; set; }

    [JsonProperty("accessExpirationInMinute")]
    public int AccessExpirationInMinute { get; set; }

    [JsonProperty("refreshExpiration")]
    public int RefreshExpiration { get; set; }
}
