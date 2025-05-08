using Application.Abstractions;
using Repository.Common;
using Repository.Repositories;

namespace Application.Services;

public class ProgramImageService(IFileUploadService fileUploadService, IFitnessProgramRepository fitnessProgramRepository, IUnitOfWork unitOfWork) : IProgramService
{
    private readonly IFileUploadService _fileUploadService = fileUploadService;
    private readonly IFitnessProgramRepository _fitnessProgramRepository = fitnessProgramRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<string> UploadProgramImageAsync(UploadProgramImageDto dto)
    {
        var fileName = await _fileUploadService.UploadAsync(dto.ProgramImage);

        var program = await _fitnessProgramRepository.GetByIdAsync(dto.ProgramId);
        if (program == null)
            throw new Exception("Program not found.");

        var file = new Domain.Entities.File
        {
            FileName = dto.ProgramImage.FileName,
            FilePath = fileName,
            FileSize = dto.ProgramImage.Length,
            FileType = dto.ProgramImage.ContentType,
            CreatedDate = DateTime.Now
        };

        await _unitOfWork.FileUploadRepository.AddAsync(file);
        await _unitOfWork.SaveChangeAsync();

        program.ImageId = file.Id - 1;
        await _unitOfWork.SaveChangeAsync();

        return fileName;
    }
}
