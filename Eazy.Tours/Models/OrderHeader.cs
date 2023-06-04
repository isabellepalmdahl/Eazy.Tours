using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Eazy.Tours.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public DateTime DateOfBooking { get; set; }
        public double OrderTotal { get; set; }
        public string? BookingStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public DateTime DateOfPayment { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

    }
}
