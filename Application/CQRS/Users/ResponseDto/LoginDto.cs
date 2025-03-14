namespace Application.CQRS.Users.ResponseDto;

public class LoginDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
