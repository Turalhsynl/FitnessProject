using Application.Services;

namespace Application.Abstractions;

public interface ICategoryService
{
    Task<string> UploadCategoryImageAsync(UploadCategoryImageDto dto);
}
