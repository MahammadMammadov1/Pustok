using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;

namespace Pustok.Repositories.Implementations
{
    public class SliderRepository : ISliderRepository
    {
        private readonly AppDbContext _appDb;

        public SliderRepository(AppDbContext appDb)
        {
            _appDb = appDb;
        }
        public async Task<List<Slider>> All()
        {
            return await _appDb.Sliders.ToListAsync();
        }

        public async Task Create(Slider slider)
        {
            await _appDb.Sliders.AddAsync(slider);
        }

        public void Delete(Slider slider)
        {
            _appDb.Sliders.Remove(slider);
        }

        public Task<Slider> GetById(int id)
        {
            return _appDb.Sliders.FirstOrDefaultAsync(slider => slider.Id == id);
        }

        public async Task<int> Save()
        {
            return await _appDb.SaveChangesAsync();
        }

        }
    }

