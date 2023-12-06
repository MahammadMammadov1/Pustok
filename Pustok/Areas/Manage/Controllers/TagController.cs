using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Services.Interfaces;
using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TagController : Controller
    {
        
        private readonly ITagService _tagService;
        private readonly ITagRepository _tagrepo;

        public TagController(ITagService tagService,ITagRepository tagrepo )
        {
            
            _tagService = tagService;
            this._tagrepo = tagrepo;
        }
        public IActionResult Index()
        {
            List<Tag> tags = _tagrepo.Table.ToList();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
            if(!ModelState.IsValid) return View(tag);


            await _tagService.Create(tag); 
            

            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var wanted = _tagrepo.GetByIdAsync(x => x.Id == id);
            if (wanted == null) return NotFound();
            return View(wanted);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Tag tag)
        {
            if (ModelState.IsValid) return View(tag);
            await _tagService.UpdateAsync(tag);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var wanted = _tagrepo.GetByIdAsync(x=>x.Id == id);
            if (wanted == null) return NotFound();
            return View(wanted);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Tag tag)
        {
            await _tagService.Delete(tag.Id);
            return RedirectToAction("Index");
        }
    }
}
