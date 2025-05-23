using Application.DTOs;

namespace Application.Abstractions;

public interface IGoogleAuthService
{
    Task<GoogleUserInfoDto> GetUserInfoAsync(string code);
}