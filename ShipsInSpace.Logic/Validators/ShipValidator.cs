using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GalacticSpaceTransitAuthority;

namespace ShipsInSpace.Logic.Validators
{
    public class ShipValidator
    {
        public static IEnumerable<string> Validate(Ship ship)
        {
            foreach (var s in ValidateWeight(ship)) yield return s;

            foreach (var s in ValidateEnergy(ship)) yield return s;

            foreach (var s in ValidateWings(ship)) yield return s;

            foreach (var s in ValidateWeapons(ship)) yield return s;
        }

        public static IEnumerable<string> ValidateWeight(Ship ship)
        {
            var totalWeight = GetShipWeight(ship);

            if (totalWeight > (int) ship.Hull.DefaultMaximumTakeOffMass)
            {
                yield return "The total weight of the Engine, Wings and Weapons exceeds the Hulls maximum take off mass.";
            }
        }

        public static IEnumerable<string> ValidateEnergy(Ship ship)
        {
            var shipEnergy = GetShipEnergy(ship);
            var weapons = GetShipWeapons(ship);

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
            var weaponTypes = GetShipWeapons(ship).Select(weapon => weapon.DamageType).Distinct().ToList();
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

        private static double GetShipWeight(Ship ship)
        {
            double weight = ship.Engine.Weight + ship.Wings.Sum(wing => wing.Weight + wing.Hardpoint.Sum(weapon => weapon.Weight));

            if (ship.Wings.SelectMany(wing => wing.Hardpoint).Count(weapon => weapon.DamageType == DamageTypeEnum.Statis) >= 2)
            {
                weight *= 0.85; // Ik neem aan dat uitrustingstukken alle soorten zijn (Engine, Wings and Weapons)
            }

            return weight;
        }

        private static double GetShipEnergy(Ship ship)
        {
            var weaponsByType = GetShipWeapons(ship).GroupBy(weapon => weapon.DamageType);

            double shipEnergy = 0;

            foreach (var typeWeapons in weaponsByType)
            {
                double energy = typeWeapons.Sum(weapon => weapon.EnergyDrain);

                if (typeWeapons.Count() >= 3)
                {
                    energy *= 0.8; // Ik neem aan dat de energy van alle Weapons omlaag gaat
                }

                shipEnergy += energy;
            }

            return shipEnergy;
        }

        private static IEnumerable<Weapon> GetShipWeapons(Ship ship)
        {
            return ship.Wings.SelectMany(wing => wing.Hardpoint);
        }
    }
}