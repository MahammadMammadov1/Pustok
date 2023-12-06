
using Pustok.Models;
using Pustok.Repositories.Interfaces;
using Pustok.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Pustok.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task CreateAsync(Author entity)
        {
            await _authorRepository.CreateAsync(entity);
            await _authorRepository.CommitAsync();
        }

        public async Task Delete(int id)
        {
            var auth =await _authorRepository.GetByIdAsync(x=>x.Id == id && x.IsDeleted == false);
            if (auth == null) throw new NullReferenceException();
             _authorRepository.DeleteAsync(auth);
            await _authorRepository.CommitAsync();

        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _authorRepository.GetAllAsync();
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            var entity = await _authorRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);
            if (entity is null) throw new NullReferenceException();
            return entity;
        }

        public async Task UpdateAsync(Author entity)
        {
            var auth = await _authorRepository.GetByIdAsync(x=>x.Id == entity.Id && x.IsDeleted == false);
            if (auth == null) throw new NullReferenceException();

            if (_authorRepository.Table.Any(x => x.Name == entity.Name && auth.Id != entity.Id))
                throw new NullReferenceException();

            auth.Name = entity.Name;
            await _authorRepository.CommitAsync();

        }
    }
}
