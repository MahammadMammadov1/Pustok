using Pustok.Core.Models;
using Pustok.DAL;

namespace Pustok.ViewService
{
    public class LayoutService
    {
        private readonly AppDbContext _appDb;
        public LayoutService(AppDbContext context)
        {
            _appDb = context;
        }

        public async Task<List<Setting>> GetBook()
        {
            var settings = _appDb.Settings.ToList();
            return settings;
        }

    }
}
