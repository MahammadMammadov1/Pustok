using Pustok.Models;
using PustokSliderCRUD.Models;

namespace Pustok.Services
{
    public interface IBookService
    {
        Task Create(Book book);
        Task DeleteAsync(int id);
        Task<List<Book>> GetAllAsync();
        Task<Book> GetAsync(int id);
        Task UpdateAsync(Book book);
        
    }
}
