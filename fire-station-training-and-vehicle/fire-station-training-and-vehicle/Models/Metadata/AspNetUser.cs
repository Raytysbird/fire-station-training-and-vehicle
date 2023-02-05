using fire_station_training_and_vehicle.CustomValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace fire_station_training_and_vehicle.Models
{
    [ModelMetadataType(typeof(AspNetUserMetadata))]
    public partial class AspNetUser
    {
    }
    public class AspNetUserMetadata
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        [FutureDateValidation]
        public DateTime? DateOfBirth { get; set; }
        public bool? IsPasswordChanged { get; set; }
        [Required]
        public string? Address { get; set; }
        public int StationId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
