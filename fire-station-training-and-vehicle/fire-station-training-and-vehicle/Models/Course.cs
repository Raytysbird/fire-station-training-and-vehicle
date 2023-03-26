using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Renewal Period")]
        public string? RenewalPeriod { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}
