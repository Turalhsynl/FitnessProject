using Application.Abstractions;
using Application.Security;
using Repository.Common;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileUploadService _fileUploadService;
    private readonly IUserContext _userContext;

    public UserService(IUnitOfWork unitOfWork, IFileUploadService fileUploadService, IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _fileUploadService = fileUploadService;
        _userContext = userContext;
    }

    public async Task<int> UploadProfileImageAsync(UploadProfileImageDto dto)
    {
        if (dto.ProfileImage == null || dto.ProfileImage.Length == 0)
            throw new Exception("Profil şəkli boşdur.");

        // Şəkili yükləyirik və file entity-ni əldə edirik
        var fileName = await _fileUploadService.UploadAsync(dto.ProfileImage);

        // Şəkil entity-sini DB-yə əlavə edirik
        var fileEntity = new Domain.Entities.File
        {
            FilePath = fileName, // FilePath burada şəkil yolunu saxlayır
            FileName = Path.GetFileName(fileName),
            FileSize = dto.ProfileImage.Length,
            FileType = dto.ProfileImage.ContentType,
            CreatedBy = _userContext.UserId,  // Əgər userContext var
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.FileUploadRepository.AddAsync(fileEntity);
        await _unitOfWork.SaveChangeAsync();

        // İndi fileEntity.Id-ni istifadəçinin ProfileImageId-sinə təyin edirik
        var userId = _userContext.UserId; // Hal-hazırda daxil olan istifadəçi
        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

        if (user == null)
            throw new Exception("İstifadəçi tapılmadı.");

        // Profil şəkilinin ID-sini istifadəçi profilinə əlavə edirik
        user.ProfileImageId = fileEntity.Id - 1;  // Burada artıq şəkilin ID-sini istifadə edirik
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangeAsync();

        return user.ProfileImageId ?? 0;  // Şəkilin ID-sini qaytarırıq
    }
}
