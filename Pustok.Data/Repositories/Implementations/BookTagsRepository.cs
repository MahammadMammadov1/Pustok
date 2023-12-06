using Pustok.DAL;
using Pustok.Models;
using Pustok.Repositories.Interfaces;

namespace Pustok.Repositories.Implementations
{
    public class BookTagsRepository : GenericRepository<BookTag>, IBookTagsRepository
    {
        public BookTagsRepository(AppDbContext appDb) : base(appDb)
        {
        }
    }
}
