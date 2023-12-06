using PustokSliderCRUD.Models;

namespace Pustok.Models
{
    public class BookImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public bool? isPoster { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
