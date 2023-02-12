using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class Document
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Description { get; set; }
        public int TypeId { get; set; }
        public bool? Status { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }

        public virtual RequestType Type { get; set; } = null!;
        public virtual AspNetUser? User { get; set; }
    }
}
