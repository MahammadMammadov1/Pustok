using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustok.Core.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string BirthDate { get; set; }
        public string?  CoonectionId { get; set; }

        public List<Order> Order { get; set; }
        public List<BasketItem> BasketItems { get; set; }
    }
}
