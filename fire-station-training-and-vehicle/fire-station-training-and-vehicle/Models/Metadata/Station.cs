using fire_station_training_and_vehicle.CustomValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace fire_station_training_and_vehicle.Models
{
    [ModelMetadataType(typeof(StationMetadata))]
    public partial class Station
    {
    }
    public class StationMetadata
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Not a valid phone number")]
        public string? PhoneNumber { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
