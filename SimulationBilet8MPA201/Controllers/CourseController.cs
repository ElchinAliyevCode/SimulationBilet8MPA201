using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationBilet8MPA201.Contexts;
using SimulationBilet8MPA201.ViewModels;

namespace SimulationBilet8MPA201.Controllers;

public class CourseController : Controller
{
    private readonly SimulationDbContext _context;

    public CourseController(SimulationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses.Select(x => new CourseGetVM()
        {
            Title = x.Title,
            Rating = x.Rating,
            ImagePath = x.ImagePath,
            TeacherFirstName = x.Teacher.FirstName,
            TeacherLastName = x.Teacher.LastName
        }).ToListAsync();
        return View(courses);
    }
}
