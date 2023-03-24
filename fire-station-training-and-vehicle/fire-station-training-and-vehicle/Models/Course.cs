using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class Course
    {
        public Course()
        {
            Events = new HashSet<Event>();
            UserTasks = new HashSet<UserTask>();
        }

        public int CourseId { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }
        public string? RenewalPeriod { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}
