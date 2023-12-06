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

        public BookController(AppDbContext appDb,IBookRepository bookRepository,IBookService bookService)
        {
            _appDb = appDb;
            _bookRepository = bookRepository;
            _bookService = bookService;
        }
        public async  Task<IActionResult> Index()
        {

            var book = await _bookRepository.GetAllAsync();
            return View(book);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();

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
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();

            if (!ModelState.IsValid) return View();
            var existBook =await _bookRepository.GetByIdAsync(x=>x.Id==id,"BookImages");
            return View(existBook);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Book book)
        {
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();

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
            ViewBag.Authors = await _bookRepository.GetAllAsync();
            ViewBag.Genres = await _bookRepository.GetAllAsync();
            ViewBag.Tags = await _bookRepository.GetAllAsync();

            if (id == null) return NotFound();

            try
            {
                await _bookService.Delete(id);
            }
            catch (Exception)
            {

            }

            return Ok();
        }

            
        
    }
}
