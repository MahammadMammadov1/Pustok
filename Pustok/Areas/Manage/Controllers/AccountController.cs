using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Areas.ViewModels;
using Pustok.Core.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel adminLoginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser admin = null;
            admin = await _userManager.FindByNameAsync(adminLoginVM.Username);

            if (admin == null)
            {
                ModelState.AddModelError("", "Invalid Username or Password");
                return View();
            }
            var result =await _signInManager.PasswordSignInAsync(admin, adminLoginVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Username or Password");
                return View();
            }
            return RedirectToAction("Index","Dashboard");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            HttpContext.SignOutAsync();
            return RedirectToAction("login", "account");
        }
        public async Task<IActionResult> CreateAdmin()
        {
            AppUser user = null;

            user = new AppUser
            {
                UserName = "SuperAdmin",
                FullName = "Mehemmed",
                Email = "mehemmedmemmedov240@gmail.com"
            };
            var result = await _userManager.CreateAsync(user, "Admin123@");
            return Ok("yarandi");
        }

        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role1 = new IdentityRole("SuperAdmin");
            IdentityRole role2 = new IdentityRole("Admin");
            IdentityRole role3 = new IdentityRole("Member");

            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            await _roleManager.CreateAsync(role3);

            return Ok("yarandi");

        }
        public async Task<IActionResult> AddRoleAdmin()
        {
            AppUser admin = await _userManager.FindByEmailAsync("mehemmedmemmedov240@gmail.com");

            await _userManager.AddToRoleAsync(admin, "SuperAdmin");
            return Ok("add olundu");
        }
    }
}
