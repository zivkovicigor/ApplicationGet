using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ApplicationGet.Models
{

    public class User : IdentityUser
    {
        public List<Reservation>? ListOfReservations { get; set; }
    }
}
