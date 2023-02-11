using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using fire_station_training_and_vehicle.CustomValidation;

namespace fire_station_training_and_vehicle.Models
{
    public class User: IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        [FutureDateValidation]
        public DateTime? DateOfBirth { get; set; }
        public bool IsPasswordChanged { get; set; }
        [Required]
        public string? Address { get; set; }
        public int? StationId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
