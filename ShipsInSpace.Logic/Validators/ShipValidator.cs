using System;
using System.Collections.Generic;
using System.Linq;
using GalacticSpaceTransitAuthority;
using ShipsInSpace.Logic.Extensions;
using ShipsInSpace.Logic.Licenses;

namespace ShipsInSpace.Logic.Validators
{
    public class ShipValidator
    {
        public static IEnumerable<string> Validate(Ship ship, PilotLicense license)
        {
            foreach (var s in ValidateHull(ship, license)) yield return s;

            foreach (var s in ValidateEngine(ship)) yield return s;

            foreach (var s in ValidateWings(ship)) yield return s;

            foreach (var s in ValidateWeapons(ship)) yield return s;
        }

        #region Hull

        public static IEnumerable<string> ValidateHull(Ship ship, PilotLicense license)
        {
            if (!ValidLicense(ship, license)) yield return "The total weight of the Engine, Wings and Weapons exceeds the maximum weight the pilot can handle.";

            if (!ValidMaximumTakeOffMass(ship)) yield return "The total weight of the Engine, Wings and Weapons exceeds the Hulls maximum take off mass.";
        }

        public static bool ValidLicense(Ship ship, PilotLicense license)
        {
            return ship.GetWeight() <= license.GetMaxWeight();
        }

        public static bool ValidMaximumTakeOffMass(Ship ship)
        {
            return ship.GetWeight() <= (int) ship.Hull.DefaultMaximumTakeOffMass;
        }

        #endregion

        #region Engine

        public static IEnumerable<string> ValidateEngine(Ship ship)
        {
            if (!ValidEnergyConsumption(ship)) yield return "The Engine and Wings cannot provide enough energy to power the Wings and Weapons.";

            if (!ValidIntrepidImploder(ship)) yield return "The Intrepid Class Engine cannot be used while using an Imploder Weapon.";
        }

        public static bool ValidEnergyConsumption(Ship ship)
        {
            return ship.GetEnergyConsumption() <= ship.Energy;
        }

        public static bool ValidIntrepidImploder(Ship ship)
        {
            return !(ship.Engine.Name == "Intrepid Class" && ship.GetWeapons().Any(weapon => weapon.Name == "Imploder"));
        }

        #endregion

        #region Wings

        public static IEnumerable<string> ValidateWings(Ship ship)
        {
            if (!ValidWingCount(ship.Wings)) yield return "The Ship must contain an even number of Wings.";

            foreach (var wing in ship.Wings.Where(wing => !ValidWingHardpointCount(wing))) yield return $"The {wing.Name} Wing contains too many Weapons. Maximum: {wing.NumberOfHardpoints}.";
        }

        public static bool ValidWingCount(IEnumerable<Wing> wings)
        {
            return wings.Count() >= 2 && wings.Count() % 2 == 0;
        }

        public static bool ValidWingHardpointCount(Wing wing)
        {
            return wing.Hardpoint.Count <= wing.NumberOfHardpoints;
        }

        #endregion

        #region Weapons

        public static IEnumerable<string> ValidateWeapons(Ship ship)
        {
            if (!ValidHeatColdCombination(ship.GetWeapons())) yield return "The combination of a Heat Weapon and a Cold Weapon is not allowed.";

            if (!ValidStatisGravityCombination(ship.GetWeapons())) yield return "The combination of a Statis Weapons and a Gravity Weapon is not allowed.";

            if (!ValidKineticWings(ship.Wings)) yield return "The difference in energy drain between Wings with Kinetic Weapons needs to be smaller than 35";

            foreach (var wing in ship.Wings)
                if (!ValidNullifierCount(wing))
                    yield return $"The Nullifier Weapon cannot be the only Weapon on the {wing.Name} Wing.";
        }

        public static bool ValidHeatColdCombination(IEnumerable<Weapon> weapons)
        {
            return ValidWeaponCombination(weapons, DamageTypeEnum.Heat, DamageTypeEnum.Cold);
        }

        public static bool ValidStatisGravityCombination(IEnumerable<Weapon> weapons)
        {
            return ValidWeaponCombination(weapons, DamageTypeEnum.Statis, DamageTypeEnum.Gravity);
        }

        public static bool ValidKineticWings(IEnumerable<Wing> wings)
        {
            var kineticWings = wings.Where(wing => wing.Hardpoint.Any(weapon => weapon.DamageType == DamageTypeEnum.Kinetic)).ToList();

            foreach (var kineticWing in kineticWings)
            {
                var kineticWingEnergy = kineticWing.Hardpoint.Where(weapon => weapon.DamageType == DamageTypeEnum.Kinetic).Sum(weapon => weapon.EnergyDrain);

                foreach (var kineticWing1 in kineticWings)
                {
                    var kineticWing1Energy = kineticWing1.Hardpoint.Where(weapon => weapon.DamageType == DamageTypeEnum.Kinetic).Sum(weapon => weapon.EnergyDrain);

                    var difference = Math.Abs(kineticWingEnergy - kineticWing1Energy);

                    if (difference >= 35) return false;
                }
            }

            return true;
        }

        public static bool ValidNullifierCount(Wing wing)
        {
            return !(wing.Hardpoint.Count == 1 && wing.Hardpoint.First().Name == "Nullifier");
        }

        public static bool ValidWeaponCombination(IEnumerable<Weapon> weapons, params DamageTypeEnum[] damageTypes)
        {
            var weaponTypes = weapons.Select(weapon => weapon.DamageType).Distinct().ToList();

            return damageTypes.Any(damageType => !weaponTypes.Contains(damageType));
        }

        #endregion
    }
}