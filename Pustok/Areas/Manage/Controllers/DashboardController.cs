using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Core.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="SuperAdmin,Admin,Editor")]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DashboardController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }


        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser()
        //    {
        //        FullName = "Mehemmed",
        //        UserName = "SuperAdmin",
        //        BirthDate = "15 avqust"
        //    };

        //    var result = await _userManager.CreateAsync(admin,"Admin123@");

        //    if (result.Succeeded)
        //    {

        //    }


        //    return Ok(result);
        //}

        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole role1 = new IdentityRole("SuperAdmin");
        //    IdentityRole role2= new IdentityRole("Admin");
        //    IdentityRole role3 = new IdentityRole("Editor");
        //    IdentityRole role4 = new IdentityRole("Member");

        //    await _roleManager.CreateAsync(role1);
        //    await _roleManager.CreateAsync(role2);
        //    await _roleManager.CreateAsync(role3);
        //    await _roleManager.CreateAsync(role4);

        //    return Ok("yarandi");
        //}

        //public async Task<IActionResult> AddRoleAdmin()
        //{
        //    AppUser admin =await _userManager.FindByNameAsync( "SuperAdmin" );

        //    await _userManager.AddToRoleAsync(admin, "SuperAdmin");
        //    return Ok("add olundu");
        //}



    }




}
