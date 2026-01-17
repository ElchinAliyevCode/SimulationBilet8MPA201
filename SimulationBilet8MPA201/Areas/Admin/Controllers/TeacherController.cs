using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationBilet8MPA201.Contexts;
using SimulationBilet8MPA201.Models;
using SimulationBilet8MPA201.ViewModels;
using System.Threading.Tasks;

namespace SimulationBilet8MPA201.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles ="Admin")]
public class TeacherController : Controller
{
    private readonly SimulationDbContext _context;

    public TeacherController(SimulationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var teachers = await _context.Teachers.Select(x => new TeacherGetVM()
        {
            Id=x.Id,
            FirstName=x.FirstName,
            LastName=x.LastName,
        }).ToListAsync();
        return View(teachers);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TeacherCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        Teacher teacher = new Teacher()
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
        };

        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if(teacher == null)
        {
            return NotFound();
        }

        TeacherUpdateVM vm = new TeacherUpdateVM()
        {
            Id = teacher.Id,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TeacherUpdateVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var teacher=await _context.Teachers.FirstOrDefaultAsync(x=>x.Id==vm.Id);
        if (teacher == null)
        {
            return NotFound();
        }

        teacher.FirstName = vm.FirstName;
        teacher.LastName = vm.LastName;

        _context.Teachers.Update(teacher);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
