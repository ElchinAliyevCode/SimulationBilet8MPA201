using System.ComponentModel.DataAnnotations;

namespace SimulationBilet8MPA201.ViewModels;

public class CourseUpdateVM
{
    public int Id { get; set; }
    [Required, MaxLength(256)]
    public string Title { get; set; }
    [Required, Range(0, 5)]
    public double Rating { get; set; }
    public IFormFile? Image { get; set; }
    [Required]
    public int TeacherId { get; set; }
}
