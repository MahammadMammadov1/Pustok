using Pustok.Business.Services.Interfaces;
using Pustok.Models;
using Pustok.Repositories;
using Pustok.Repositories.Implementations;
using Pustok.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustok.Business.Services.Implementations
{
    

    public class TagService : ITagService
    {
        private readonly ITagRepository tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        public async Task Create(Tag tag)
        {
            if (tagRepository.Table.Any(x => x.Name == tag.Name))
                throw new NullReferenceException();
            await tagRepository.CreateAsync(tag);
            await tagRepository.CommitAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await tagRepository.GetByIdAsync(x => x.Id == id && x.IsDeleted == false);
            if (entity == null) throw new NullReferenceException();

            tagRepository.DeleteAsync(entity);
            await tagRepository.CommitAsync();
        }

        public Task<List<Genre>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Genre> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Tag tag)
        {
            var gen = await tagRepository.GetByIdAsync(x => x.Id == tag.Id && x.IsDeleted == false);
            if (gen is null) throw new NullReferenceException();

            if (tagRepository.Table.Any(x => x.Name == tag.Name && gen.Id != tag.Id))
                throw new NullReferenceException();

            gen.Name = tag.Name;
            tagRepository.CommitAsync();
        }
    }
}
