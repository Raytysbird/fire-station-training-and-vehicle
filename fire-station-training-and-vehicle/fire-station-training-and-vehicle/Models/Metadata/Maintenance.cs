using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace fire_station_training_and_vehicle.Models
{
    [ModelMetadataType(typeof(MaintenanceMetadata))]
    public partial class Maintenance
    {
    }
    public class MaintenanceMetadata
    {
        public int RepairId { get; set; }
        [Required]
        [Display(Name = "Vehicle")]
        public int? VehicleId { get; set; }
       
        [Display(Name = "Date of Repair")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public DateTime? DateOfRepair { get; set; }
        [Required]
        [Display(Name = "Issue")]
        public string? Description { get; set; }
        public DateTime? DateCompleted { get; set; }
        [Required]
        [Display(Name = "Mieleage")]
        public int? Milage { get; set; }
        [Required]
        [Display(Name = "Special Notes")]
        public string? Notes { get; set; }
        public string? Status { get; set; }
    }
}
