using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Models
{
    public class CompanyUpdateDto:IValidatableObject
    {
        [DisplayName("公司名称")]
        [Required(ErrorMessage = "{0}是必填项")]
        [MaxLength(100)]
        public string Name { get; set; }
        [DisplayName("简介")]
        [Required(ErrorMessage = "{0}是必填项")]
        [StringLength(maximumLength: 500, MinimumLength = 5, ErrorMessage = "{0}的长度范围从{2}到{1}")]
        public string Introduction { get; set; }
        public ICollection<EmployeeAddDto> employees { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Length > 10)
            {
                yield return new ValidationResult("IValidatableObject验证失败，姓名过长！", new[] { nameof(Name) });
            }
        }
    }
}
