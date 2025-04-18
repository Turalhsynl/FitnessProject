using Domain.BaseEntities;
using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public Decimal Height { get; set; }
    public Decimal Weight { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRoles UserRole { get; set; }

    public int? MembershipPlanId { get; set; }
    public MembershipPlan MembershipPlan { get; set; }
    public ICollection<UserProgram> PurchasedProqrams { get; set; }
    public int? ProfileImageId { get; set; }
    public File? ProfileImage { get; set; }

}
