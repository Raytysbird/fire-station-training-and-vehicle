using fire_station_training_and_vehicle.CustomValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace fire_station_training_and_vehicle.Models
{
    [ModelMetadataType(typeof(VehicleMetadata))]
    public partial class Vehicle
    {
    }
    public class VehicleMetadata
    {
        public int VehicleId { get; set; }
        [Required]
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeId { get; set; }
        [Required]
        [Display(Name = "Station")]
        public int StationId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Licence Plate")]
        public string? LicencePlate { get; set; }
        [Required]
        [Display(Name = "Licence Expiry")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public DateTime? LicenceExpiry { get; set; }
     
        public string? VehicleStatus { get; set; }
        [Required]
        public string? Make { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        [RegularExpression(@"([0-9]{4})", ErrorMessage = "Not a valid year")]
        public int? Year { get; set; }
        public bool? IsDeleted { get; set; }
     
    }
}
