using FinalExam.Contexts;
using FinalExam.Extensions;
using FinalExam.Models;
using FinalExam.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductController(FinalDbContext _context, IWebHostEnvironment _env) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.Include(x => x.Department).ToListAsync());
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Department = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
        if (vm.ProfileImage != null)
        {
            if (!vm.ProfileImage.IsValidType("image"))
                ModelState.AddModelError("File", "File must be an image");
            if (!vm.ProfileImage.IsValidSize(400))
                ModelState.AddModelError("File", "File must be less than 400");
        }
        if (!ModelState.IsValid)
        {
            ViewBag.Department = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
            return View(vm);
        }
        if (!await _context.Departments.AnyAsync(x => x.Id == vm.DepartmentId && !x.IsDeleted))
        {
            ViewBag.Department = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
            ModelState.AddModelError("DepartmentId", "Department not found");
        }
        var impPath = Path.Combine(_env.WebRootPath, "imgs", "products");
        Product product = new Product
        {
            FullName = vm.FullName,
            ProfileImageUrl = impPath,
            DepartmentId = vm.DepartmentId,
        };
        if (vm.ProfileImage != null)
        {
            product.ProfileImageUrl = await vm.ProfileImage.UploadAsync(impPath);
        }
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null) return BadRequest();
        var data = await _context.Products.Where(x => x.Id == id).Select(x => new ProductUpdateVM
        {
            FullName = x.FullName,
            ProfileImageUrl = x.ProfileImageUrl,
            DepartmentId = x.DepartmentId,
        }).FirstOrDefaultAsync();
        if (data == null) return NotFound();
        ViewBag.Department = await _context.Departments.Where(x => !x.IsDeleted).ToListAsync();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
    {
        if (id == null) return BadRequest();
        var data = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (data == null) return NotFound();
        var impUpd = Path.Combine(_env.WebRootPath, "imgs", "products");
        data.FullName = vm.FullName;
        data.ProfileImageUrl = impUpd;
        data.DepartmentId = vm.DepartmentId;
        if (vm.ProfileImage != null)
        {
            data.ProfileImageUrl = await vm.ProfileImage.UploadAsync(impUpd);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return BadRequest();
        var data = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (data == null) return NotFound();
        _context.Products.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
