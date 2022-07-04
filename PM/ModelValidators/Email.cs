using PM.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Vidly.ModelValidators
{
    public class Email : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var institue = (institute)validationContext.ObjectInstance;
            var email = institue.email; 


              var pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
+ "@"
+ @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";


            if (email == null) return new ValidationResult("Email is required");

            return Regex.IsMatch(email , pattern)
                ? ValidationResult.Success 
                : new ValidationResult("Email is not valid");
        }
    }
}