using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Favorites.Handlers.AddFavorite;
using static Application.CQRS.Favorites.Handlers.GetFavorites;
using static Application.CQRS.Favorites.Handlers.RemoveFavorite;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FavoriteController : ControllerBase
{
    private readonly ISender _sender;

    public FavoriteController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteCommand command)
    {
        var result = await _sender.Send(command);
        if (result)
            return Ok(new { message = "Favoriye eklendi." });
        else
            return BadRequest(new { message = "Favori eklenirken bir hata oluştu." });
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemoveFavorite([FromBody] RemoveFavoriteCommand command)
    {
        var result = await _sender.Send(command);
        if (result)
            return Ok(new { message = "Favoriden çıkarıldı." });
        else
            return BadRequest(new { message = "Favori silinirken bir hata oluştu." });
    }

    [HttpGet("list/{userId}")]
    public async Task<IActionResult> GetFavorites(int userId)
    {
        var query = new GetFavoritesQuery { UserId = userId };
        var result = await _sender.Send(query);
        return Ok(result);
    }
}
