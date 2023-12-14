using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.ViewModels;
using Pustok.Core.Models;
using Pustok.DAL;
using Pustok.Migrations;
using Pustok.ViewModels;

namespace Pustok.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly AppDbContext _appDb;

		public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole > roleManager,
			SignInManager<AppUser> signInManager,
			AppDbContext appDb)
        {
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_appDb = appDb;
		}
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> Register(MemberRegisterViewModel memberRegisterVM)
		{
            if(!ModelState.IsValid) return View();
            AppUser user = null;

            user = await _userManager.FindByNameAsync(memberRegisterVM.Username);
            if(user != null) 
            {
                ModelState.AddModelError("Username", "Username already exist");
                return View();
            }

            user = await _userManager.FindByEmailAsync(memberRegisterVM.Email); 
            if(user != null)
            {
				ModelState.AddModelError("Email", "Email already exist");
				return View();
			}
            AppUser user1 = new AppUser
            {
                FullName = memberRegisterVM.Fullname,
                UserName = memberRegisterVM.Username,
                Email = memberRegisterVM.Email,
                BirthDate = memberRegisterVM.BirthDate,
            };

            var result = await _userManager.CreateAsync(user1,memberRegisterVM.Password);

            if(result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
					ModelState.AddModelError("", item.Description);
					return View();
				}
            }
            await _userManager.AddToRoleAsync(user1, "Member");

			return RedirectToAction("Index","Home");
		}
		public IActionResult Login()
		{

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(MemberLoginViewModel memberLoginVM)
		{
			if (!ModelState.IsValid) return View();
			AppUser admin = null;
			admin = await _userManager.FindByEmailAsync(memberLoginVM.Email);

			if (admin == null)
			{
				ModelState.AddModelError("", "Invalid Email or Password");
				return View();
			}
			var result = await _signInManager.PasswordSignInAsync(admin, memberLoginVM.Password, false, false);

			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Invalid Email or Password");
				return View();
			}
			return RedirectToAction("Index", "home");
		}
        [HttpPost]
        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

		[Authorize(Roles = "SuperAdmin,Admin,Member")]
		public async Task<IActionResult> Profile()
		{
			AppUser appUser = null;

			if (HttpContext.User.Identity.IsAuthenticated)
			{
				appUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
			}

			List<Order> orders = await _appDb.Orders.Where(x => x.AppUserId == appUser.Id).ToListAsync();

			return View(orders);
		}
	}
}
