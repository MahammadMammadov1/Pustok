using PustokSliderCRUD.Models;

namespace Pustok.ViewModels
{
    public class ProductDetailViewModel
    {
        public Book Book { get; set; }

        public List<Book> ReleatedBooks { get; set; }
    }
}
