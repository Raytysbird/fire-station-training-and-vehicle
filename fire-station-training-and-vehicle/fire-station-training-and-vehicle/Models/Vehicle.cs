using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            VehicleReports = new HashSet<VehicleReport>();
        }

        public int VehicleId { get; set; }
        public int? VehicleTypeId { get; set; }
        public int? StationId { get; set; }
        public string? Name { get; set; }
        public string? LicencePlate { get; set; }
        public DateTime? LicenceExpiry { get; set; }
        public string? VehicleStatus { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Station? Station { get; set; }
        public virtual VehicleType? VehicleType { get; set; }
        public virtual ICollection<VehicleReport> VehicleReports { get; set; }
    }
}
