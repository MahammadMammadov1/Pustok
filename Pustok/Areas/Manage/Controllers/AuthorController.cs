using Microsoft.AspNetCore.Mvc;
using Pustok.DAL;
using Pustok.Models;
using Pustok.Services.Interfaces;

namespace Pustok.Areas.Manage.Controllers
{
    
        [Area("Manage")]
        public class AuthorController : Controller
        {
            private readonly AppDbContext _appDb;
        private readonly IAuthorService _authorService;

        public AuthorController(AppDbContext context,IAuthorService authorService)
            {
                _appDb = context;
            _authorService = authorService;
        }
            public async Task<IActionResult> Index()
            {
                var author =await _authorService.GetAllAsync();
                return View(author);
            }
            public IActionResult Create()
            {
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> Create(Author author)
            {
                if (!ModelState.IsValid) return View(author);
                
                await _authorService.CreateAsync(author);
                return RedirectToAction("Index");
            }
            public async Task<IActionResult> Update(int id)
            {
                var wanted = await _authorService.GetByIdAsync(id);
                if (wanted == null) return NotFound();
                return View(wanted);
            }
            [HttpPost]
            public async Task<IActionResult> Update(Author author)
            {
                Author existAuthor = await _authorService.GetByIdAsync(author.Id);
                if (existAuthor == null) return NotFound();
                if (!ModelState.IsValid) return View(existAuthor);
               
                await _authorService.UpdateAsync(author);
                return RedirectToAction("Index");
            }
            public async  Task<IActionResult> Delete(int id)
            {
                var wanted = await _authorService.GetByIdAsync(id);
                if (wanted == null) return NotFound();
                return View(wanted);
            }
            [HttpPost]
            public async Task<IActionResult> Delete(Author author)
            {
                var wanted = await _authorService.GetByIdAsync(author.Id);
                if (wanted == null) return NotFound();
                _appDb.Authors.Remove(wanted);
                _appDb.SaveChanges();
                return RedirectToAction("Index");
            }
        }
}
