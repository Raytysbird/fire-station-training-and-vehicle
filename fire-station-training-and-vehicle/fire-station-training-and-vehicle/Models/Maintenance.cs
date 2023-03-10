using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class Maintenance
    {
        public int RepairId { get; set; }
        public int? VehicleId { get; set; }
        public DateTime? DateOfRepair { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int? Milage { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }

        public virtual Vehicle? Vehicle { get; set; }
    }
}
