using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GalacticSpaceTransitAuthority;
using Microsoft.Extensions.DependencyInjection;

namespace ShipsInSpace.Logic.Validators.Attributes
{
    public class WeaponsExist : ValidationAttribute
    {
        public override bool RequiresValidationContext => true;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var transitAuthority = validationContext.GetService<ISpaceTransitAuthority>();

            if (value is not IEnumerable<int> weapons) return new ValidationResult($"The {validationContext.DisplayName} is not a weapon list.");

            foreach (var weaponId in weapons)
                if (!transitAuthority.GetWeapons().Any(weapon => weapon.Id == weaponId))
                    return new ValidationResult($"Weapon {weaponId} does not exist in the SpaceTransitAuthority");

            return ValidationResult.Success;
        }
    }
}