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
            if (value!=null)
            {
                DateTime today = DateTime.Now;
                int year = int.Parse(today.ToString("yy"));
                int month = int.Parse(today.ToString("MM"));
                int date= int.Parse(today.ToString("dd"));

                string ex = value.ToString();

                DateTime inputDate = DateTime.Parse(ex);

                int inputYear = int.Parse(inputDate.ToString("yy"));
                int inputMonth = int.Parse(inputDate.ToString("MM"));
                int inputDay = int.Parse(inputDate.ToString("dd"));
                
                
              
                if (inputYear > year)
                {
                    return new ValidationResult(string.Format(ErrorMessage,validationContext.DisplayName));
                }
                if (inputYear == year && inputMonth > month )
                {
                    return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
                }
                if (inputYear == year && inputMonth == month)
                {
                    if (inputDay > date)
                    {
                        return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
                    }
                }
            }       
                 return ValidationResult.Success;
        }
    }
}
