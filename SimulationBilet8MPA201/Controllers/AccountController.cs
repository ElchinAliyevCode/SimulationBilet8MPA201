using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimulationBilet8MPA201.Models;
using SimulationBilet8MPA201.ViewModels;

namespace SimulationBilet8MPA201.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var existEmail = await _userManager.FindByEmailAsync(vm.Email);
        if (existEmail != null)
        {
            ModelState.AddModelError("", "Email already exist");
            return View(vm);
        }

        var existUserName = await _userManager.FindByEmailAsync(vm.UserName);
        if (existUserName != null)
        {
            ModelState.AddModelError("", "Username already exist");
            return View(vm);
        }

        AppUser user = new AppUser()
        {
            Email = vm.Email,
            UserName = vm.UserName
        };

        var result = await _userManager.CreateAsync(user, vm.Password);
        if (!result.Succeeded)
        {
            return BadRequest();
        }

        result = await _userManager.AddToRoleAsync(user, "Member");
        if (!result.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return BadRequest();
        }

        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var user = await _userManager.FindByEmailAsync(vm.Email);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.CheckPasswordAsync(user, vm.Password);
        if (!result)
        {
            ModelState.AddModelError("", "Email or password is wrong");
            return View(vm);
        }

        await _signInManager.SignInAsync(user, false);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> CreateRoles()
    {
        var adminRole = new IdentityRole()
        {
            Name = "Admin"
        };

        var memberRole = new IdentityRole()
        {
            Name = "Member"
        };

        var result = await _roleManager.CreateAsync(adminRole);
        if (!result.Succeeded)
        {
            return BadRequest();
        }

        result = await _roleManager.CreateAsync(memberRole);
        if (!result.Succeeded)
        {
            return BadRequest();
        }

        return Ok("Created");
    }
}
