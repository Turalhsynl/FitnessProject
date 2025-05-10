namespace Application.CQRS.Recipes.ResponseDto;

public class RecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Ingredients { get; set; }
    public string Instructions { get; set; }
    public int? ImageId { get; set; }
    public int Calories { get; set; }
    public string MealType { get; set; }
}
