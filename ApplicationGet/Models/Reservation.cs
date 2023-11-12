using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ApplicationGet.Models
{
    public class Reservation
    {
 
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }

        public int Id { get; set; }

        public int NumberOfSeats { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";
        [Required]

        [ForeignKey("FlightId")]
        [ValidateNever]
        public Flight Flight { get; set; }
    }
}
