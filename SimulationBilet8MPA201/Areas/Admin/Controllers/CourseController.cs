using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationBilet8MPA201.Contexts;
using SimulationBilet8MPA201.Helpers;
using SimulationBilet8MPA201.Models;
using SimulationBilet8MPA201.ViewModels;
using System.Threading.Tasks;

namespace SimulationBilet8MPA201.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles ="Admin")]
public class CourseController : Controller
{
    private readonly SimulationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _folderPath;
    public CourseController(SimulationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _folderPath = Path.Combine(_environment.WebRootPath, "img");
    }

    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses.Select(x => new CourseGetVM()
        {
            Id = x.Id,
            Title = x.Title,
            Rating = x.Rating,
            ImagePath = x.ImagePath,
            TeacherFirstName = x.Teacher.FirstName,
            TeacherLastName = x.Teacher.LastName
        }).ToListAsync();
        return View(courses);
    }

    public async Task<IActionResult> Create()
    {
        await SendTeacherWithViewBag();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateVM vm)
    {
        await SendTeacherWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existTeacher = await _context.Teachers.AnyAsync(x => x.Id == vm.TeacherId);
        if (!existTeacher)
        {
            ModelState.AddModelError("", "Teacher not found");
            return View(vm);
        }

        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("", "Max 2mb");
            return View(vm);
        }

        if (!vm.Image.CheckType("image"))
        {
            ModelState.AddModelError("", "Image must be in correct format");
            return View(vm);
        }

        var uniqueName = await vm.Image.UploadFileAsync(_folderPath);

        Course course = new Course()
        {
            Title = vm.Title,
            Rating = vm.Rating,
            TeacherId = vm.TeacherId,
            ImagePath = uniqueName
        };

        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        CourseUpdateVM vm = new CourseUpdateVM()
        {
            Id = course.Id,
            Title = course.Title,
            Rating = course.Rating,
            TeacherId = course.TeacherId,
        };

        await SendTeacherWithViewBag();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CourseUpdateVM vm)
    {
        await SendTeacherWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == vm.Id);
        if (course == null)
        {
            return NotFound();
        }

        var existTeacher = await _context.Teachers.AnyAsync(x => x.Id == vm.TeacherId);
        if (!existTeacher)
        {
            ModelState.AddModelError("", "Teacher not found");
            return View(vm);
        }

        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("", "Max 2mb");
            return View(vm);
        }

        if (!vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("", "Image must be in correct format");
            return View(vm);
        }

        course.Title = vm.Title;
        course.Rating = vm.Rating;
        course.TeacherId = vm.TeacherId;

        if (vm.Image is { })
        {
            var uniqueName = await vm.Image.UploadFileAsync(_folderPath);
            var deletedPath = Path.Combine(_folderPath, course.ImagePath);
            FileHelper.DeleteFile(deletedPath);
            course.ImagePath = uniqueName;
        }

        _context.Courses.Update(course);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Delete(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        var deletedPath = Path.Combine(_folderPath, course.ImagePath);
        FileHelper.DeleteFile(deletedPath);

        return RedirectToAction(nameof(Index));
    }

    private async Task SendTeacherWithViewBag()
    {
        var teachers = await _context.Teachers.ToListAsync();
        ViewBag.Teachers = teachers;
    }
}
