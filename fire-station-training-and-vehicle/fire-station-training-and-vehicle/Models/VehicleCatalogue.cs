using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class VehicleCatalogue
    {
        public int DefaultTypeId { get; set; }
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        public int? TankMinimumCapacity { get; set; }
        public bool? HeavyRescue { get; set; }
        public int? HoseOneHalfInch { get; set; }
        public int? HoseOneInch { get; set; }
        public int? HoseTwoHalfInch { get; set; }
        public bool? Ladders { get; set; }
        public bool? MasterStream { get; set; }
        public int? MaximumGvr { get; set; }
        public int? MinPersonel { get; set; }
        public bool? PumpAndRoll { get; set; }
        public int? PumpMinimumFlow { get; set; }
        public int? RatedPressure { get; set; }
        public bool? Turntable { get; set; }
        public string? TypicalUse { get; set; }
        public bool? WildLandRescue { get; set; }
        public bool? Structure { get; set; }
    }
}
