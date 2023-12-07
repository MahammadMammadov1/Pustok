using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Exceptions;
using Pustok.Models;
using Pustok.Repositories.Interfaces;
using Pustok.Services;

using PustokSliderCRUD.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        private readonly AppDbContext _appDb;
        private readonly IBookRepository _bookRepository;
        
        private readonly IBookService _bookService;
        private readonly ITagRepository _tagRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookController(AppDbContext appDb,
            IBookRepository bookRepository,
            IBookService bookService,
            ITagRepository tagRepository,
            IGenreRepository genreRepository,
            IAuthorRepository authorRepository
            )
        {
            _appDb = appDb;
            _bookRepository = bookRepository;
            _bookService = bookService;
            _tagRepository = tagRepository;
            _genreRepository = genreRepository;
            _authorRepository = authorRepository;
        }
        public async  Task<IActionResult> Index()
        {

            var book = await _bookRepository.GetAllAsync();
            return View(book);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _authorRepository.GetAllAsync();
            ViewBag.Genres = await _genreRepository.GetAllAsync();
            ViewBag.Tags = await _tagRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            ViewBag.Authors = await _authorRepository.GetAllAsync();
            ViewBag.Genres = await _genreRepository.GetAllAsync();
            ViewBag.Tags = await _tagRepository.GetAllAsync();

            if (!ModelState.IsValid) return View(book);

            try
            {
                await _bookRepository.CreateAsync(book);    
                await _appDb.SaveChangesAsync();    
            }
            catch (TotalBookExceptions ex)
            {
                ModelState.AddModelError(ex.Prop,ex.Message);   
                
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Authors = await _authorRepository.GetAllAsync();
            ViewBag.Genres = await _genreRepository.GetAllAsync();
            ViewBag.Tags = await _tagRepository.GetAllAsync();

            if (!ModelState.IsValid) return View();
            var existBook =await _bookRepository.GetByIdAsync(x=>x.Id==id,"BookImages","Author","Genre","BookTags.Tag");
            return View(existBook);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {
            ViewBag.Authors = await _authorRepository.GetAllAsync();
            ViewBag.Genres = await _genreRepository.GetAllAsync();
            ViewBag.Tags = await _tagRepository.GetAllAsync();

            if (!ModelState.IsValid) return View(book);

            try
            {
                await _bookService.UpdateAsync(book);   
            }
            catch (TotalBookExceptions ex)
            {
                ModelState.AddModelError(ex.Prop, ex.Message);
                
            }

            await _bookRepository.CommitAsync();
            return RedirectToAction("Index");
        }
       
        [HttpGet]
        public async Task< IActionResult> Delete(int id)
        {
            ViewBag.Authors = await _authorRepository.GetAllAsync();
            ViewBag.Genres = await _genreRepository.GetAllAsync();
            ViewBag.Tags = await _tagRepository.GetAllAsync();

            if (id == null) return NotFound();

            try
            {
                await _bookService.DeleteAsync(id);
            }
            catch (Exception)
            {

            }

            return Ok();
        }

            
        
    }
}
