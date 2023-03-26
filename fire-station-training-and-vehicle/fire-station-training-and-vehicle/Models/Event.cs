using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fire_station_training_and_vehicle.Models
{
    public partial class Event
    {
        public Event()
        {
            AssignedEvents = new HashSet<AssignedEvent>();
        }

        public int EventId { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        public string? Location { get; set; }
        [Display(Name = "Teacher")]
        public string? TeacherId { get; set; }
        [Display(Name = "Course")]
        public int? CourseId { get; set; }

        public virtual Course? Course { get; set; }
        public virtual ICollection<AssignedEvent> AssignedEvents { get; set; }
    }
}
