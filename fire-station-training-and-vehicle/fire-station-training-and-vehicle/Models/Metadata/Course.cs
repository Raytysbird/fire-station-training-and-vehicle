using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace fire_station_training_and_vehicle.Models
{
    [ModelMetadataType(typeof(CourseMetadata))]
    public partial class Course
    {
    }
    public class CourseMetadata
    {
        public int CourseId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Details { get; set; }
        [Required]
        public string? RenewalPeriod { get; set; }
       

    }
}
