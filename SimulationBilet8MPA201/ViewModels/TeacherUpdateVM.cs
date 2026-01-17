using System.ComponentModel.DataAnnotations;

namespace SimulationBilet8MPA201.ViewModels;

public class TeacherUpdateVM
{
    public int Id { get; set; }
    [Required, MaxLength(256)]
    public string FirstName { get; set; }
    [Required, MaxLength(256)]
    public string LastName { get; set; }
}
