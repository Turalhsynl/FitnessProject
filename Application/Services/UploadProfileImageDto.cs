using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Services;

public class UploadProfileImageDto
{
    [Required]
    public IFormFile ProfileImage { get; set; }
}
