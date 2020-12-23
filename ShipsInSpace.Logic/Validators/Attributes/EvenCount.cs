using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ShipsInSpace.Logic.Validators.Attributes
{
    public class EvenCount : ValidationAttribute
    {
        public override bool RequiresValidationContext => true;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IEnumerable<object> list)
            {
                return new ValidationResult($"The {validationContext.DisplayName} is not a list.");
            }

            if (list.Count() % 2 != 0)
            {
                return new ValidationResult($"The {validationContext.DisplayName} does not contain an even amount of {validationContext.DisplayName}.");
            }

            return ValidationResult.Success;
        }
    }
}