using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.ViewModels;

namespace Pustok.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _appDb;

        public ShopController(AppDbContext appDb)
        {
            this._appDb = appDb;
        }
        public async Task<IActionResult> Index(int? genreId)
        {
            var query = _appDb.Books.Include(x => x.Author).Include(x => x.BookImages).AsQueryable();
            if (genreId != null)
            {
                query = query.Where(x=>x.GenreId == genreId);

            }
            ShopViewModel model = new ShopViewModel
            {
                Books =await query.ToListAsync(),
                Genres =await _appDb.Genres.Include(x=>x.Books).ToListAsync()
            };



            return View(model);
        }
    }
}
