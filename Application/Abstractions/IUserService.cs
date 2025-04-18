using Application.Services;

namespace Application.Abstractions;

public interface IUserService
{
    Task<int> UploadProfileImageAsync(UploadProfileImageDto dto);
}
