using Application.Abstractions;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController(IFileUploadService fileUploadService) : ControllerBase
{
    private readonly IFileUploadService _fileUploadService = fileUploadService;

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] UploadFileDto model)
    {
        if (model.File == null || model.File.Length == 0)
            return BadRequest("Fayl boşdur.");

        var fileName = await _fileUploadService.UploadAsync(model.File);

        return Ok(new { IsSuccess = true, FileName = fileName });
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var file = await _fileUploadService.GetByIdAsync(id);
        return file is null ? NotFound() : Ok(file);
    }

    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var file = await _fileUploadService.GetByNameAsync(name);
        return file is null ? NotFound() : Ok(file);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        await _fileUploadService.RemoveAsync(id);
        return NoContent();
    }

    [HttpGet("download-path/{fileName}")]
    public async Task<IActionResult> DownloadByPath(string fileName)
    {
        var filePath = await _fileUploadService.GetFilePathAsync(fileName);
        var contentType = "application/octet-stream";
        return PhysicalFile(filePath, contentType, fileName);
    }

    [HttpGet("download-bytes/{fileName}")]
    public async Task<IActionResult> DownloadByBytes(string fileName)
    {
        var fileBytes = await _fileUploadService.DownloadAsBytesAsync(fileName);
        var contentType = "application/octet-stream";
        return File(fileBytes, contentType, fileName);
    }
}
