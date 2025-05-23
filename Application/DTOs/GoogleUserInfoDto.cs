using System.Text.Json.Serialization;

namespace Application.DTOs;

public class GoogleUserInfoDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("given_name")]
    public string Given_name { get; set; } = string.Empty;

    [JsonPropertyName("family_name")]
    public string Family_name { get; set; } = string.Empty;

    // Əgər lazım olsa, başqa property-lər də əlavə edə bilərsən
}
