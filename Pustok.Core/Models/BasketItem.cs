using Pustok.Models;
using PustokSliderCRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustok.Core.Models
{
    public class BasketItem : BaseEntity
    {
        public string AppUserId { get; set; }
        public int BookId { get; set; }
        public int Count { get; set; }
        public Book Book { get; set; }
        public AppUser AppUser  { get; set; }

    }
}
