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

        public static IEnumerable<string> ValidateHull(Ship ship, PilotLicense license)
        {
            var totalWeight = ship.GetWeight();

            if (totalWeight > license.GetMaxWeight())
            {
                yield return "The total weight of the Engine, Wings and Weapons exceeds the maximum weight the pilot can handle.";
            }

            if (totalWeight > (int) ship.Hull.DefaultMaximumTakeOffMass)
            {
                yield return "The total weight of the Engine, Wings and Weapons exceeds the Hulls maximum take off mass.";
            }
        }

        public static IEnumerable<string> ValidateEngine(Ship ship)
        {
            var shipEnergy = ship.GetEnergyConsumption();
            var weapons = ship.GetWeapons();

            if (shipEnergy > ship.Energy)
            {
                yield return "The Engine cannot provide enough energy to power the Wings and Weapons.";
            }

            if (ship.Engine.Name == "Intrepid Class" && weapons.Any(weapon => weapon.Name == "Imploder"))
            {
                yield return "The Intrepid Class Engine cannot be used while using an Imploder Weapon.";
            }
        }

        public static IEnumerable<string> ValidateWings(Ship ship)
        {
            if (ship.Wings.Count % 2 != 0)
            {
                yield return "The Ship must contain an even number of Wings.";
            }

            foreach (var wing in ship.Wings.Where(wing => wing.Hardpoint.Count > wing.NumberOfHardpoints))
            {
                yield return $"The {wing.Name} Wing contains too many Weapons. Maximum: {wing.NumberOfHardpoints}.";
            }
        }

        public static IEnumerable<string> ValidateWeapons(Ship ship)
        {
            var weaponTypes = ship.GetWeapons().Select(weapon => weapon.DamageType).Distinct().ToList();
            var kineticWings = ship.Wings.Where(wing => wing.Hardpoint.Any(weapon => weapon.DamageType == DamageTypeEnum.Kinetic)).ToList();

            if (weaponTypes.Contains(DamageTypeEnum.Heat) && weaponTypes.Contains(DamageTypeEnum.Cold))
            {
                yield return "The combination of a Heat Weapon and a Cold Weapon is not allowed.";
            }

            if (weaponTypes.Contains(DamageTypeEnum.Statis) && weaponTypes.Contains(DamageTypeEnum.Gravity))
            {
                yield return "The combination of a Statis Weapons and a Gravity Weapon is not allowed.";
            }

            foreach (var kineticWing in kineticWings)
            {
                var kineticWingEnergy = kineticWing.Hardpoint.Where(weapon => weapon.DamageType == DamageTypeEnum.Kinetic).Sum(weapon => weapon.EnergyDrain);

                foreach (var kineticWing1 in kineticWings)
                {
                    var kineticWing1Energy = kineticWing1.Hardpoint.Where(weapon => weapon.DamageType == DamageTypeEnum.Kinetic).Sum(weapon => weapon.EnergyDrain);

                    var difference = Math.Abs(kineticWingEnergy - kineticWing1Energy);

                    if (difference >= 35)
                    {
                        yield return "The difference in energy drain between Wings with Kinetic Weapons needs to be smaller than 35";

                        goto KineticWingsEnd;
                    }
                }
            }

            KineticWingsEnd:

            if (ship.Wings.Any(wing => wing.Hardpoint.Count == 1 && wing.Hardpoint.First().Name == "Nullifier"))
            {
                yield return "The Nullifier Weapon cannot be the only Weapon on a Wing.";
            }
        }
    }
}