using Routine.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.ValidationAttributes
{
    public class CompanyValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addDto = (CompanyAddDto)validationContext.ObjectInstance;
            if (addDto.Name== "ValidationAttribute")
            {
                return new ValidationResult(ErrorMessage,new[] { nameof(CompanyAddDto)});
            }
            return ValidationResult.Success;
        }
    }
}
