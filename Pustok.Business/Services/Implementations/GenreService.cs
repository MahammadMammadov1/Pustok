using Pustok.Models;
using Pustok.Repositories.Implementations;
using Pustok.Repositories.Interfaces;
using Pustok.Services.Interfaces;

namespace Pustok.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }
        public async Task Create(Genre genre)
        {
            if (_genreRepository.Table.Any(x => x.Name == genre.Name))
                throw new NullReferenceException();
            await _genreRepository.CreateAsync(genre);
            await _genreRepository.CommitAsync();
        }

        public async Task Delete(int id)
        {
            var entity =await  _genreRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted ==false);
            if (entity == null) throw new NullReferenceException();

            _genreRepository.DeleteAsync(entity);
            await _genreRepository.CommitAsync();
        }

        public Task<List<Genre>> GetAllAsync()
        {
            return _genreRepository.GetAllAsync();
        }

        public async Task<Genre> GetAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);
            if (entity is null) throw new NullReferenceException();
            return entity;
        }

        public async Task UpdateAsync(Genre genre)
        {
            var gen = await _genreRepository.GetByIdAsync(x=>x.Id == genre.Id && x.IsDeleted == false);
            if(gen is null) throw new NullReferenceException();

            if(_genreRepository.Table.Any(x=>x.Name == genre.Name && gen.Id != genre.Id))
                throw new NullReferenceException(); 

            gen.Name = genre.Name;
            _genreRepository.CommitAsync();
        }
    }
}
