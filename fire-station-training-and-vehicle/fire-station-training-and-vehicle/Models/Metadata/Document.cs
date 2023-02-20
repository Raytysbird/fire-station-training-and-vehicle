using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace fire_station_training_and_vehicle.Models
{
    [ModelMetadataType(typeof(DocumentMetadata))]
    public partial class Document
    {
    }
    public class DocumentMetadata
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Document Type")]
        public int RequestTypeId { get; set; }
        public bool? Status { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
    }
}
