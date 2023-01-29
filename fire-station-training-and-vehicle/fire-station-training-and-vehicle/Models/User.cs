using Microsoft.AspNetCore.Identity;

namespace fire_station_training_and_vehicle.Models
{
    public class User: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? StreetAddress { get; set; }
        public string? AptNumber { get; set; }
        public string? UnitNumber { get; set; }
        public string? Building { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Province { get; set; }
    }
}
