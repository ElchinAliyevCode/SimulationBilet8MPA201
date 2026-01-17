using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationBilet8MPA201.Contexts;
using SimulationBilet8MPA201.Models;
using SimulationBilet8MPA201.ViewModels;

namespace SimulationBilet8MPA201.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }    
    }
}
