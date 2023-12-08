using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Services.Interfaces;
using Pustok.ViewModels;

namespace Pustok.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IGenreService _genreService;
        private readonly AppDbContext _appDb;

        public HeaderViewComponent(IGenreService genreService,AppDbContext appDb)
        {
            _genreService = genreService;
            _appDb = appDb;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HeaderViewModel headerViewModel = new HeaderViewModel();

            headerViewModel.Genres = await _genreService.GetAllAsync();
            headerViewModel.Settings = _appDb.Settings.ToList();
            return View(headerViewModel);
        }
    }
}
