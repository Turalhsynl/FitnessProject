using Application.Abstractions;
using Application.Security;
using Common.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Repository.Common;
using Repository.Repositories;

namespace Application.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IFileUploadRepository _fileUploadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _rootPath;

    public FileUploadService(IFileUploadRepository fileUploadRepository, IUnitOfWork unitOfWork)
    {
        _fileUploadRepository = fileUploadRepository;
        _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        _unitOfWork = unitOfWork;
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Fayl mövcud deyil.");

        string folderPath = CreateFolder.CreateDirectoryForFile(_rootPath, DateTime.UtcNow);
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileEntity = new Domain.Entities.File
        {
            FilePath = filePath,
            FileName = fileName,
            FileSize = file.Length,
            FileType = file.ContentType,
            CreatedBy = 1,
            CreatedDate = DateTime.Now
        };



        await _unitOfWork.FileUploadRepository.AddAsync(fileEntity);
        await _unitOfWork.SaveChangeAsync();

        return fileEntity.Id.ToString();
    }

    public async Task<Domain.Entities.File> GetByIdAsync(int id)
    {
        return await _fileUploadRepository.GetByIdAsync(id);
    }

    public async Task<Domain.Entities.File> GetByNameAsync(string name)
    {
        return await _fileUploadRepository.GetByNameAsync(name);
    }

    public async Task RemoveAsync(int id)
    {
        await _fileUploadRepository.Remove(id);
    }

    public async Task<string> GetFilePathAsync(string fileName)
    {
        var file = await _fileUploadRepository.GetByNameAsync(fileName);
        if (file == null || file.IsDeleted)
            throw new Exception("Fayl tapılmadı.");

        return file.FilePath;
    }

    public async Task<byte[]> DownloadAsBytesAsync(string fileName)
    {
        var file = await _fileUploadRepository.GetByNameAsync(fileName);
        if (file == null || file.IsDeleted)
            throw new Exception("Fayl tapılmadı.");

        if (!System.IO.File.Exists(file.FilePath))
            throw new Exception("Fayl sistemdə mövcud deyil.");

        return await System.IO.File.ReadAllBytesAsync(file.FilePath);
    }
}
