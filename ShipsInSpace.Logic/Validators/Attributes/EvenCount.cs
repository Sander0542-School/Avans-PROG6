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
            if (value is int intValue && intValue % 2 == 0)
            {
                return ValidationResult.Success;
            }

            if (value is IEnumerable<object> listValue && listValue.Count() % 2 == 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} does not have an even count.");
        }
    }
}