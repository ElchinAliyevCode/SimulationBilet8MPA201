namespace SimulationBilet8MPA201.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public double Rating { get; set; }
    public string ImagePath { get; set; }
    public Teacher Teacher { get; set; }
    public int TeacherId { get; set; }
}
