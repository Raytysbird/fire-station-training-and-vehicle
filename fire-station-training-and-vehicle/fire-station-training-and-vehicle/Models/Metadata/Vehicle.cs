using Microsoft.AspNetCore.Mvc;

namespace fire_station_training_and_vehicle.Models.Metadata
{
    [ModelMetadataType(typeof(VehicleMetadata))]
    public partial class Vehicle
    {
    }
    public class VehicleMetadata
    {
        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        public int StationId { get; set; }
        public string? Name { get; set; }
        public string? LicencePlate { get; set; }
        public DateTime? LicenceExpiry { get; set; }
        public string? VehicleStatus { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public bool? IsDeleted { get; set; }
     
    }
}
