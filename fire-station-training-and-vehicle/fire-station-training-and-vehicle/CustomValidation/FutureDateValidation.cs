using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace fire_station_training_and_vehicle.CustomValidation
{
    public class FutureDateValidation : ValidationAttribute
    {
        public FutureDateValidation()
        {
            ErrorMessage = "Date of birth cannot be in future!!";
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime today = DateTime.Now;
                string ex = value.ToString();
                DateTime inputDate = DateTime.Parse(ex);
                if (inputDate > today)
                {
                    return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}
