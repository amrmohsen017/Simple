


    using PM.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vidly.ModelValidators
{
    public class Institute_name: ValidationAttribute
    {
        private project_managementEntities1 db  = new project_managementEntities1();



        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            
            var institue = (institute)validationContext.ObjectInstance;
            var name = institue.institutename;


            var exists = db.institutes.FirstOrDefault(i => i.institutename == name); 

            if(exists != null)
            {
                return new ValidationResult("Institute name is already registered");
            }


            var pattern = @"^[0-9_.-]*$";
            var symbolsPattern = "[!@#$%^&*(),.?\":{ }|<>\\]\\]|\\[\\[']";

            if (name == null) return new ValidationResult("Institute name is requried");
            //var test = Regex.IsMatch(name, symbolsPattern); 

            return Regex.IsMatch(name, pattern) && Regex.IsMatch(name, symbolsPattern)
                ? new ValidationResult("Institute name is not valid")
                : ValidationResult.Success;
        }
    }
}