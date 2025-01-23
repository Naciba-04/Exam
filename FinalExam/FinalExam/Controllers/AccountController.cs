using FinalExam.Extensions;
using FinalExam.Models;
using FinalExam.ViewModels.Auths;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalExam.Controllers;

public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager) : Controller
{
    public bool IsAuthendicated => HttpContext.User.Identity?.IsAuthenticated ?? false;
    public IActionResult Register()
    {

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        AppUser user = new AppUser
        {
            FullName = vm.FullName,
            UserName = vm.Username,
            Email = vm.Email,
        };
        var userCreate = await _userManager.CreateAsync(user, vm.Password);
        if (!userCreate.Succeeded)
        {
            foreach (var err in userCreate.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
            return View(vm);
        }
        var roleAdd = await _userManager.AddToRoleAsync(user, Roles.User.GetRole());
        if (!roleAdd.Succeeded)
        {
            foreach (var err in roleAdd.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
            return View(vm);
        }
        return RedirectToAction("Login", "Account");
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm, string? returnUrl = null)
    {
        if (IsAuthendicated) return RedirectToAction("Index", "Home");
        if (!ModelState.IsValid) return View(vm);
        AppUser user = null;
        if (vm.UserNameOrEmail.Contains("@"))
        {
            user = await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
        }
        else
        {
            user = await _userManager.FindByNameAsync(vm.UserNameOrEmail);
        }
        if (user == null)
        {
            ModelState.AddModelError("", "User tapilmadi");
        }
        var sign = await _signInManager.PasswordSignInAsync(user, vm.Password, false, true);
        if (!sign.Succeeded)
        {
            if (sign.IsLockedOut)
            {
                ModelState.AddModelError("", "Siz 1 deqiqelik bloklandiniz");
            }
            if (sign.IsNotAllowed)
            {
                ModelState.AddModelError("", "user veya pasvor yanlishdir");
            }
            return View(vm);
        }
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> MyRoleMethod()
    {
        var data = await _userManager.FindByNameAsync("admin");
        if (data == null)
        {
            AppUser user = new AppUser
            {
                FullName = "Admin",
                UserName = "admin",
                Email = "admin@mail.com",
            };
            var crt = await _userManager.CreateAsync(user, "Salam123");
            if (!crt.Succeeded)
            {
                return BadRequest("Admin yaradilmadi");
            }
            if (!await _roleManager.RoleExistsAsync(Roles.Admin.GetRole()))
            {
                var role = await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.GetRole()));
                if (!role.Succeeded)
                {
                    return BadRequest("Rol yaradilmadi");
                }
            }
            var addadmin = await _userManager.AddToRoleAsync(user, Roles.Admin.GetRole());
            if (!addadmin.Succeeded)
            {
                return BadRequest("Admin rola elave edilmedi");
            }

        }
        foreach (Roles item in Enum.GetValues(typeof(Roles)))
        {
            if(!await _roleManager.RoleExistsAsync(item.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name=item.ToString()});
            }
        }
        return RedirectToAction("Login", "Account");
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
