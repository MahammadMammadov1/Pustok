using Microsoft.EntityFrameworkCore;
using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;
using PustokSliderCRUD.Models;

namespace Pustok.Repositories.Implementations
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext appDb) : base(appDb)
        {
        }
    }
}
