using System;
using System.Collections.Generic;

namespace fire_station_training_and_vehicle.Models
{
    public partial class RequestType
    {
        public RequestType()
        {
            Documents = new HashSet<Document>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
