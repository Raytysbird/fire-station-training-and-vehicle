using fire_station_training_and_vehicle.CustomValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace fire_station_training_and_vehicle.Models
{
    [ModelMetadataType(typeof(VehicleReportMetadata))]
    public partial class VehicleReport
    {
    }
    public class VehicleReportMetadata
    {
        public int ReportId { get; set; }
        public string? UserId { get; set; }
        [Required]
        [Display(Name = "Vehicle")]
        public int? VehicleId { get; set; }
        [Required]
        [FutureDateValidation]
        [Display(Name = "Date of Report")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public DateTime? DateReported { get; set; }
        public string? Status { get; set; }
        [Required]
        public string? IssueType { get; set; }
        [Required]
        public string? Description { get; set; }
    }
    }
