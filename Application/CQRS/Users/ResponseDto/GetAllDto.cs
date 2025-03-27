using Domain.Enums;

namespace Application.CQRS.Users.ResponseDto;

public class GetAllDto
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public UserGender Gender { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
}
