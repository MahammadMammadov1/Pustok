using Pustok.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustok.Business.Services.Interfaces
{
    public interface ITagService 
    {
        Task Create(Tag genre);
        Task Delete(int id);
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetAsync(int id);
        Task UpdateAsync(Tag genre);
    }
}
