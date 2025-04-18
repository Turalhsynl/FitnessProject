using Application.Services;

namespace Application.Abstractions;

public interface IProductService
{
    Task<string> UploadProductImageAsync(UploadProductImageDto dto);
}