using System.ComponentModel.DataAnnotations;

namespace fire_station_training_and_vehicle.CustomValidation
{
    public class AgeValidation : ValidationAttribute
    {
        public AgeValidation()
        {
            ErrorMessage = "Minimum age should be 18 years!!";
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime today = DateTime.Now;
                string ex = value.ToString();
                DateTime inputDate = DateTime.Parse(ex);
                decimal days= (int)(today-inputDate).TotalDays;
                int year = (int)Math.Truncate(days / 365);
                if (year <18) {
                    return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));

                }
            }
            return ValidationResult.Success;
        }
    }
}
