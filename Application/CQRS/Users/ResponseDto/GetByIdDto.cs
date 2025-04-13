namespace Application.CQRS.Users.ResponseDto;

public class GetByIdDto
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public Decimal Height { get; set; }
    public Decimal Weight { get; set; }
}
