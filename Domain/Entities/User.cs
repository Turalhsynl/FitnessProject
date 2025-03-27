using Domain.BaseEntities;
using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public UserGender Gender { get; set; }
    public int Age { get; set; }
    public Decimal Height { get; set; }
    public Decimal Weight { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRoles UserRole { get; set; }

}
