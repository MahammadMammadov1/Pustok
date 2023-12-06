using Pustok.Models;

namespace Pustok.Services
{
    public interface ISliderService
    {
        Task CreateAsync(Slider slider);
        Task DeleteAsync(int id);
        Task<Slider> GetAsync(int id);
        Task<List<Slider>> GetAllAsync();
        Task UpdateAsync(Slider slider);

    }
}
