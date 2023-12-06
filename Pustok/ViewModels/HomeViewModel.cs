using Pustok.Models;
using PustokSliderCRUD.Models;

namespace Pustok.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Service> Services { get; set; }
        public List<Book> NewBooks { get; set; }
        public List<Book> FeaturedBooks { get; set; }
        public List<Book> BestsellerBooks { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Author> Authors { get; set; }
    }
}
