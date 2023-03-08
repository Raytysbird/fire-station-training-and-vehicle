using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class VehicleReport
    {
        public int ReportId { get; set; }
        public string? UserId { get; set; }
        public int? VehicleId { get; set; }
        public DateTime? DateReported { get; set; }
        public string? Status { get; set; }
        public string? IssueType { get; set; }
        public string? Description { get; set; }

        public virtual AspNetUser? User { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
    }
}
