using Domain.BaseEntities;

namespace Domain.Entities;

public class MembershipPlan:BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int MaxProgramsAllowed { get; set; }
    public ICollection<User> Users { get; set; }

}
