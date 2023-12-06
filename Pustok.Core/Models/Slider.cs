using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Title { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Description { get; set; }
        [StringLength(maximumLength: 100)]
        public string? ImageUrl { get; set; }

        public string RedirctUrl { get; set; }
        public string RedirctText { get; set; }

        [NotMapped]
        public IFormFile? FormFile { get; set; }

    }
}
