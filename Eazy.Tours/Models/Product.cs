using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Eazy.Tours.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        public string Address { get; set; }

        public DateTime Availability { get; set; }

        //add date for calender later
    }
}
