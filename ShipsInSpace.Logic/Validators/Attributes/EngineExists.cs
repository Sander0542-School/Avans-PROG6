using System.ComponentModel.DataAnnotations;
using System.Linq;
using GalacticSpaceTransitAuthority;
using Microsoft.Extensions.DependencyInjection;

namespace ShipsInSpace.Logic.Validators.Attributes
{
    public class EngineExists : ValidationAttribute
    {
        public override bool RequiresValidationContext => true;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var transitAuthority = validationContext.GetService<ISpaceTransitAuthority>();

            if (!int.TryParse(value.ToString(), out var engineId))
            {
                return new ValidationResult($"Engine {value} is not a valid engine id.");
            }

            if (!transitAuthority.GetEngines().Any(engine => engine.Id == engineId))
            {
                return new ValidationResult($"Engine {value} does not exist in the SpaceTransitAuthority");
            }
            
            return ValidationResult.Success;
        }
    }
}