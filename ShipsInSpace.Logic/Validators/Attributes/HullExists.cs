using System.ComponentModel.DataAnnotations;
using System.Linq;
using GalacticSpaceTransitAuthority;
using Microsoft.Extensions.DependencyInjection;

namespace ShipsInSpace.Logic.Validators.Attributes
{
    public class HullExists : ValidationAttribute
    {
        public override bool RequiresValidationContext => true;
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var transitAuthority = validationContext.GetService<ISpaceTransitAuthority>();

            if (!int.TryParse(value.ToString(), out var hullId))
            {
                return new ValidationResult($"Hull {value} is not a valid hull id.");
            }

            if (!transitAuthority.GetHulls().Any(hull => hull.Id == hullId))
            {
                return new ValidationResult($"Hull {value} does not exist in the SpaceTransitAuthority");
            }
            
            return ValidationResult.Success;
        }
    }
}