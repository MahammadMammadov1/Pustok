using Microsoft.AspNetCore.Identity;
using Pustok.Core.Models;
using Pustok.DAL;

namespace Pustok.ViewService
{
    public class LayoutService
    {
        private readonly AppDbContext _appDb;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutService(AppDbContext context,UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _appDb = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Setting>> GetBook()
        {
            var settings = _appDb.Settings.ToList();
            return settings;
        }

        public async Task<AppUser> GetAppUser() 
        {
			AppUser user = null;
			
			if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
				
			}

			return user;
		}
	}

    }

