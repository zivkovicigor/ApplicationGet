using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationGet.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }

        public int FromCityId { get; set; }
        
        [ForeignKey("FromCityId")]
        public City? FromCity { get; set; }

        public int ToCityId { get; set; }
        [ForeignKey("ToCityId")]
        public City? ToCity { get; set; }

        public string Status { get; set; } = "Active";

        public int NumberOfLayovers { get; set; }
    }
}
