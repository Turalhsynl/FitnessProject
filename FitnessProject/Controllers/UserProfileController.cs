using Application.Abstractions;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserProfileController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("upload-profile-image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProfileImage([FromForm] UploadProfileImageDto dto)
    {
        try
        {
            var profileImageId = await _userService.UploadProfileImageAsync(dto);

            return Ok(new { IsSuccess = true, ProfileImageId = profileImageId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { IsSuccess = false, ErrorMessage = ex.Message });
        }
    }
}
