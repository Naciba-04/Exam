using FinalExam.Contexts;
using FinalExam.Models;
using FinalExam.ViewModels.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DepartmentController(FinalDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await _context.Departments.ToListAsync());
    }
    public IActionResult Craete()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Craete(DepartmentCreateVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        Department department = new Department
        {
            Name = vm.Name,
        };
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null) return BadRequest();
        var data = await _context.Departments.Where(x => x.Id == id).Select(x => new DepartmentUpdateVM
        {
            Name = x.Name,
        }).FirstOrDefaultAsync();
        if (data == null) return NotFound();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id,DepartmentUpdateVM vm)
    {
        if (id == null) return BadRequest();
        var data = await _context.Departments.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (data == null) return NotFound();
        data.Name = vm.Name;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return BadRequest();
        var data = await _context.Departments.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (data == null) return NotFound();
        _context.Departments.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
