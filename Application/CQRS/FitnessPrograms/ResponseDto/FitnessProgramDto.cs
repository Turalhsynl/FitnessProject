using Application.CQRS.Recipes.ResponseDto;

namespace Application.CQRS.FitnessPrograms.ResponseDto;

public class FitnessProgramDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Level { get; set; }
    public int DurationInWeeks { get; set; }
    public string Gender { get; set; }
    public decimal Price { get; set; }
    public string VideoUrl { get; set; }
    public int? ImageId { get; set; }
    public List<RecipeDto> Recipes { get; set; }
    public int UserId { get; set; }
}
