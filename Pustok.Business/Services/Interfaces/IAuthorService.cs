using Pustok.Models;

namespace Pustok.Services.Interfaces
{
    public interface IAuthorService
    {
        Task CreateAsync(Author entity);
        Task Delete(int id);
        Task<Author> GetByIdAsync(int id);
        Task<List<Author>> GetAllAsync();
        Task UpdateAsync(Author entity);

    }
}
