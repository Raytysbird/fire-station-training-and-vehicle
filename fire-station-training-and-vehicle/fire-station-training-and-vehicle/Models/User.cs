using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace fire_station_training_and_vehicle.Models
{
    public class User: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsPasswordChanged { get; set; }
        public string? Address { get; set; }
    }
}
