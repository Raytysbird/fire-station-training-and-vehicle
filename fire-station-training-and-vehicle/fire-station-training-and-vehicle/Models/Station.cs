using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class Station
    {
        public Station()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
