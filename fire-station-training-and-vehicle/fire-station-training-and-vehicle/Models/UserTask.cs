using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class UserTask
    {
        public UserTask()
        {
            AssignedTasks = new HashSet<AssignedTask>();
        }

        public int TaskId { get; set; }
        public int? CourseId { get; set; }
        public DateTime? LastDate { get; set; }
        public bool? IsCompleted { get; set; }

        public virtual Course? Course { get; set; }
        public virtual ICollection<AssignedTask> AssignedTasks { get; set; }
    }
}
