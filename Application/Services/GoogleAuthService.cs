using Application.Abstractions;
using Application.DTOs;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Application.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public GoogleAuthService(IConfiguration config, HttpClient httpClient)
    {
        _config = config;
        _httpClient = httpClient;
    }

    public async Task<GoogleUserInfoDto> GetUserInfoAsync(string code)
    {
        var clientId = _config["GoogleAuth:ClientId"];
        var clientSecret = _config["GoogleAuth:ClientSecret"];
        var redirectUri = _config["GoogleAuth:RedirectUri"];

        // Step 1 - Exchange code for access token
        var tokenResponse = await _httpClient.PostAsync("https://oauth2.googleapis.com/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"code", code},
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"redirect_uri", redirectUri},
                {"grant_type", "authorization_code"}
            }));

        if (!tokenResponse.IsSuccessStatusCode)
        {
            var errorContent = await tokenResponse.Content.ReadAsStringAsync();
            throw new Exception($"Token alınmadı: {errorContent}");
        }

        var tokenResult = JsonSerializer.Deserialize<JsonElement>(await tokenResponse.Content.ReadAsStringAsync());
        var accessToken = tokenResult.GetProperty("access_token").GetString();

        // Step 2 - Get user info
        var userResponse = await _httpClient.GetAsync($"https://www.googleapis.com/oauth2/v2/userinfo?access_token={accessToken}");
        var userJson = await userResponse.Content.ReadAsStringAsync();

        var googleUser = JsonSerializer.Deserialize<GoogleUserInfoDto>(userJson);

        if (googleUser == null || string.IsNullOrEmpty(googleUser.Email))
        {
            throw new Exception("Google istifadəçi məlumatları düzgün alınmadı.");
        }

        return googleUser;
    }
}
