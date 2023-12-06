using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Exceptions;
using Pustok.Models;
using Pustok.Repositories;
using Pustok.Repositories.Interfaces;
using Pustok.Services;


namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly ISliderService _sliderService;

        public SliderController(ISliderRepository sliderRepository,ISliderService sliderService)
        {
            _sliderRepository = sliderRepository;
            _sliderService = sliderService;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliderList = await _sliderService.GetAllAsync();
            return View(sliderList);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);

            try
            {
                await _sliderService.CreateAsync(slider);
            }
            catch (TotalSliderExceptions ex)
            {
                ModelState.AddModelError(ex.Prop,ex.Message);
                return View();
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var wanted = await _sliderRepository.GetById(id);

            return View(wanted);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);

            try
            {
                await _sliderService.UpdateAsync(slider);
            }
            catch (TotalSliderExceptions ex)
            {
                ModelState.AddModelError(ex.Prop, ex.Message);
                return View();
            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return NotFound();

            try
            {
                await _sliderService.DeleteAsync(id);
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Index");
        }


    }
}
