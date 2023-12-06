using Pustok.Models;

namespace Pustok.Repositories
{
    public interface ISliderRepository
    {
        Task Create(Slider slider);
        
        Task<Slider> GetById(int id);
        
        Task<List<Slider>> All();
        void Delete(Slider slider);
        Task<int> Save();
    }
}
