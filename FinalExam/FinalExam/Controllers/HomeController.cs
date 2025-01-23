using FinalExam.Contexts;
using FinalExam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FinalExam.Controllers;

public class HomeController(FinalDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.Include(x => x.Department).ToListAsync());
    }
}
