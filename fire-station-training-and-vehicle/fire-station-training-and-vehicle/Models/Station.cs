﻿using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class Station
    {
        public Station()
        {
            AspNetUsers = new HashSet<AspNetUser>();
            Vehicles = new HashSet<Vehicle>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
