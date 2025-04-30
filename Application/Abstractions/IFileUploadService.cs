using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions;

public interface IFileUploadService
{
    Task<string> UploadAsync(IFormFile file);
    Task<Domain.Entities.File> GetByIdAsync(int id);
    Task<Domain.Entities.File> GetByNameAsync(string name);
    Task RemoveAsync(int id);
    Task<string> GetFilePathAsync(string fileName);
    Task<byte[]> DownloadAsBytesAsync(string fileName);
}
