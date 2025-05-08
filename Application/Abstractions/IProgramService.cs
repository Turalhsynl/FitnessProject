using Application.Services;

namespace Application.Abstractions;

public interface IProgramService
{
    Task<string> UploadProgramImageAsync(UploadProgramImageDto dto);
}
