using Pustok.Models;

namespace Pustok.Services.Interfaces
{
    public interface IGenreService 
    {
        Task Create(Genre genre);
        Task Delete(int id);
        Task <List<Genre>> GetAllAsync();
        Task<Genre> GetAsync(int id);
        Task UpdateAsync(Genre genre);
    }
}
