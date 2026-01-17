using System.ComponentModel.DataAnnotations;

namespace SimulationBilet8MPA201.ViewModels;

public class TeacherCreateVM
{
    [Required,MaxLength(256)]
    public string FirstName { get; set; }
    [Required, MaxLength(256)]
    public string LastName { get; set; }
}
