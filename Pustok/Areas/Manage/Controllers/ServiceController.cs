using Microsoft.AspNetCore.Mvc;
using Pustok.DAL;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _service;

        public ServiceController(AppDbContext appDb)
        {
            _service = appDb;
        }
        public IActionResult Index()
        {
            List<Service> sliderList = _service.Services.ToList();
            return View(sliderList);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Service Service)
        {
            _service.Services.Add(Service);
            _service.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var wanted = _service.Services.FirstOrDefault(x => x.Id == id);

            return View(wanted);
        }
        [HttpPost]
        public IActionResult Update(Service slider)
        {
            var wanted = _service.Services.FirstOrDefault(x => x.Id == slider.Id);

            wanted.Title = slider.Title;
            wanted.Description = slider.Description;
            wanted.Logo = slider.Logo;
            

            _service.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var wanted = _service.Services.FirstOrDefault(x => x.Id == id);

            return View(wanted);
        }
        [HttpPost]
        public IActionResult Delete(Service slider)
        {
            var wanted = _service.Services.FirstOrDefault(x => x.Id == slider.Id);
            _service.Services.Remove(wanted);
            _service.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
