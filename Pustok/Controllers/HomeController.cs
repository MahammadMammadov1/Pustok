using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using Pustok.ViewModels;
using System.Diagnostics;

namespace Pustok.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _logger;

        public HomeController(AppDbContext logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.Sliders = _logger.Sliders.ToList();
            model.Services = _logger.Services.ToList();
            model.NewBooks = _logger.Books.Include(b => b.BookImages).Include(a => a.Author).Where(b => b.isNew).ToList();
            model.FeaturedBooks = _logger.Books.Include(b => b.BookImages).Include(a => a.Author).Where(b => b.isFeatured).ToList();
            model.BestsellerBooks = _logger.Books.Include(b => b.BookImages).Include(a => a.Author).Where(b => b.isBestseller).ToList();

            return View(model);
        }

       
        
    }
}