using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using Pustok.Services.Interfaces;

namespace PustokSliderCRUD.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class GenreController : Controller
    {
        private readonly AppDbContext _appDb;
        private readonly IGenreService _genreService;

        public GenreController(AppDbContext context,IGenreService genreService)
        {
            _appDb = context;
            this._genreService = genreService;
        }
        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllAsync();
            return View(genres);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Genre genre)
        {
            if (!ModelState.IsValid) return View(genre);
            //if (_appDb.Genres.Any(x => x.Name.ToLower().Trim() == genre.Name.ToLower().Trim()))
            //{
            //    ModelState.AddModelError("Name", "Genre alredy exist!!!");
            //    return View(genre);
            //}
            //_appDb.Genres.Add(genre);
            //_appDb.SaveChanges();

            _genreService.Create(genre);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            Genre wanted = await _genreService.GetAsync(id);
            if (wanted == null) return NotFound();
            return View(wanted);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Genre genre)
        {
            //Genre existGenre = _appDb.Genres.FirstOrDefault(x => x.Id == genre.Id);
            //if (existGenre == null) return NotFound();
            if (!ModelState.IsValid) return View();
            
            await _genreService.UpdateAsync(genre);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            //Genre genre = _appDb.Genres.FirstOrDefault(g => g.Id == id);

            //if (genre == null) return NotFound();
            //_appDb.Genres.Remove(genre);
            //_appDb.SaveChanges();

            _genreService.Delete(id);

            return Ok();
        }
    }
}