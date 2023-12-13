using Pustok.Core.Models;

namespace Pustok.ViewModels
{
    public class OrderViewModel
    {
        public List<CheckOutViewModel> CheckOutVM { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string? Note { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        
        public List<OrderItem> OrderItems { get; set; }
    }
}
