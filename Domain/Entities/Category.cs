using Domain.BaseEntities;

namespace Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    //public List<Product> Products { get; set; }
    //public Category()
    //{
    //    Products = new List<Product>();
    //}
}
