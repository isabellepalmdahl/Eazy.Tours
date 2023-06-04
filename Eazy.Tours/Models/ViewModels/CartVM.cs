using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Eazy.Tours.Models.ViewModels
{
    public class CartVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [ValidateNever]
        public Product Product { get; set; }

        [ValidateNever]
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public int Count { get; set; }

        public int Availability { get; set; }
        public IEnumerable<Cart> ListOfCart { get; set; } = new List<Cart>();
        public OrderHeader OrderHeader { get; set; }
        public Cart Cart { get; set; } //?

    }
}
