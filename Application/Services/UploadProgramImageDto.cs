using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class UploadProgramImageDto
{
    public IFormFile? ProgramImage { get; set; }
    public int ProgramId { get; set; }
}
