using System.ComponentModel.DataAnnotations;
using System.Linq;
using GalacticSpaceTransitAuthority;
using Microsoft.Extensions.DependencyInjection;

namespace ShipsInSpace.Logic.Validators.Attributes
{
    public class WingExists : ValidationAttribute
    {
        public override bool RequiresValidationContext => true;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var transitAuthority = validationContext.GetService<ISpaceTransitAuthority>();

            if (!int.TryParse(value.ToString(), out var wingId))
            {
                return new ValidationResult($"Wing {value} is not a valid wing id.");
            }

            if (!transitAuthority.GetWings().Any(wing => wing.Id == wingId))
            {
                return new ValidationResult($"Wing {value} does not exist in the SpaceTransitAuthority");
            }
            
            return ValidationResult.Success;
        }
    }
}