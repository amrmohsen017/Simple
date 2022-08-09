using PM.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vidly.ModelValidators
{
    public class ValidateMe : ValidationAttribute
    {

        private project_managementEntities1 db = new project_managementEntities1();


        private string _fieldName;
       
        public ValidateMe( string fieldName)
        {
            _fieldName = fieldName;
     
         
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {




            dynamic type = null;
            if (validationContext.ObjectInstance is institute)
            {
                type = (institute)validationContext.ObjectInstance;


            }

            else if (validationContext.ObjectInstance is institute_address)
            {
                type = (institute_address)validationContext.ObjectInstance;

            }

            else if (validationContext.ObjectInstance is department)
            {
                type = (department)validationContext.ObjectInstance;

            }
            else if (validationContext.ObjectInstance is user)
            {
                type = (user)validationContext.ObjectInstance;

            }

            else if (validationContext.ObjectInstance is section)
            {
                type = (section)validationContext.ObjectInstance;

            }
            else if (validationContext.ObjectInstance is institute_type)
            {
                type = (institute_type)validationContext.ObjectInstance;

            }

            else if (validationContext.ObjectInstance is station)
            {
                type = (station)validationContext.ObjectInstance;

            }

            else if (validationContext.ObjectInstance is city)
            {
                type = (city)validationContext.ObjectInstance;

            }

            else if (validationContext.ObjectInstance is governmnet)
            {
                type = (governmnet)validationContext.ObjectInstance;

            }
            else if (validationContext.ObjectInstance is task)
            {
                type = (task)validationContext.ObjectInstance;

            }

            var symbolsPattern = "[!@#$%^&*(),.?\":{}|<>\\]\\]|\\[\\[']";


            if (_fieldName == "email")
            {

                var email = type.email;


                var pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
    + "@"
    + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";


                if (email == null) return new ValidationResult("Email is required");

                return Regex.IsMatch(email, pattern)
                    ? ValidationResult.Success
                    : new ValidationResult("Email is not valid");

            }


            else if (_fieldName == "institutename")
            {
                var name = (type as institute).institutename; // A LIL HACK to the copmiler :) 

                var id = (type as institute).institute_id; // for the records update bypass :) 

                if (name == null) return new ValidationResult("Institute name is requried");


                var exists = db.institutes.FirstOrDefault(i => i.institutename == name);
                if ((type as institute).institute_id == 0)

                {
                    if (id == 0 && exists != null)
                    {
                        return new ValidationResult("Institute name is already registered");
                    }
                }



                var pattern = @"^[0-9_.-]*$";


                return Regex.IsMatch(name, pattern) || Regex.IsMatch(name, symbolsPattern)
                        ? new ValidationResult("Institute name is not valid")
                        : ValidationResult.Success;

            }



            else if (_fieldName == "username")
            {
                var name = (type as user).username; // A LIL HACK to the copmiler :) 
                if (name == null) return new ValidationResult("Username is requried");

                var exists = db.users.FirstOrDefault(i => i.username == name);
                if ((type as user).user_id == 0)

                {
                    if (exists != null)
                    {
                        return new ValidationResult("Username is already registered");
                    }

                }
                return Regex.IsMatch(name, symbolsPattern)
                        ? new ValidationResult("username is not valid")
                        : ValidationResult.Success;




            }

            else if (_fieldName == "pass")
            {
                var name = (type as user).pass; // A LIL HACK to the copmiler :) 
                if (name == null) return new ValidationResult("Password is requried");

                var exists = db.users.FirstOrDefault(i => i.username == name);


                return ValidationResult.Success;

            }

            else if (_fieldName == "departmentname")
            {
                var name = (type as department).departmentname; // A LIL HACK to the copmiler :) 
                if (name == null) return new ValidationResult("Department name is requried");

                var exists = db.departments.FirstOrDefault(i => i.departmentname == name);
                if ((type as department).departementid == 0)

                {
                    if (exists != null)
                    {
                        return new ValidationResult("Department is already registered");
                    }
                }

                var pattern = @"^[0-9_.-]*$";

                return Regex.IsMatch(name, symbolsPattern) || Regex.IsMatch(name, pattern)
                        ? new ValidationResult("department name is not valid")
                        : ValidationResult.Success;

            }
            else if (_fieldName == "typename")
            {
                var name = (type as institute_type).typename; // A LIL HACK to the copmiler :) 

                if (name == null) return new ValidationResult("Institute name is requried");
                if ((type as institute_type).type_id == 0)

                {
                    var exists = db.institute_type.FirstOrDefault(i => i.typename == name);

                    if (exists != null)
                    {
                        return new ValidationResult("Institute is already registered");
                    }

                }
                var pattern = @"^[0-9_.-]*$";

                return Regex.IsMatch(name, symbolsPattern) || Regex.IsMatch(name, pattern)
                        ? new ValidationResult("type name is not valid")
                        : ValidationResult.Success;
            }

            else if (_fieldName == "sectionname")
            {
                var name = (type as section).sectionname; // A LIL HACK to the copmiler :) 

                if (name == null) return new ValidationResult("Section name is requried");
                if ((type as section).section_id == 0)

                {
                    var exists = db.sections.FirstOrDefault(i => i.sectionname == name);

                    if (exists != null)
                    {
                        return new ValidationResult("Section is already registered");
                    }

                }
                var pattern = @"^[0-9_.-]*$";

                return Regex.IsMatch(name, symbolsPattern) || Regex.IsMatch(name, pattern)
                        ? new ValidationResult("type name is not valid")
                        : ValidationResult.Success;
            }

            else if (_fieldName == "telephone")
            {




                var telephone = (type as user) == null ? (type as institute).telephone : (type as user).telephone;


                if (telephone == null) return new ValidationResult("telephone is requried");

                return (telephone.Length < 11 || telephone.Length > 15) || Regex.IsMatch(telephone, symbolsPattern)
                       ? new ValidationResult("telephone is not valid")
                       : ValidationResult.Success;

            }

            else if (_fieldName == "task_name")
            {

                var task_name = (type as task).task_name;

                if (task_name == null ) return new ValidationResult("task_name is requried");
                if ((type as task).task_id == 0)

                {
                    var exists = db.tasks.FirstOrDefault(i => i.task_name == task_name);

                    if (exists != null)
                    {
                        return new ValidationResult("Task is already registered");
                    }

                }


                return  ValidationResult.Success;

            }



                return new ValidationResult("Not handled case :) ");


        }
    }
}