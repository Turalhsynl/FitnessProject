using Application.Abstractions;
using Repository.Common;
using Repository.Repositories;

namespace Application.Services;

public class ProductService(IFileUploadService fileUploadService, IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
{
    private readonly IFileUploadService _fileUploadService = fileUploadService;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<string> UploadProductImageAsync(UploadProductImageDto dto)
    {
        var fileName = await _fileUploadService.UploadAsync(dto.ProductImage);

        var product = await _productRepository.GetByIdAsync(dto.ProductId);
        if (product == null)
            throw new Exception("Məhsul tapılmadı.");

        var file = new Domain.Entities.File
        {
            FileName = dto.ProductImage.FileName,
            FilePath = fileName,
            FileSize = dto.ProductImage.Length,
            FileType = dto.ProductImage.ContentType,
            CreatedDate = DateTime.Now
        };


        await _unitOfWork.FileUploadRepository.AddAsync(file);
        await _unitOfWork.SaveChangeAsync();

        product.ImageId = file.Id - 1;
        await _unitOfWork.SaveChangeAsync();

        return fileName;
    }
}
