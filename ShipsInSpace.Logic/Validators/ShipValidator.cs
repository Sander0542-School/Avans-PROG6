using System.Collections.Generic;
using System.Linq;
using GalacticSpaceTransitAuthority;

namespace ShipsInSpace.Logic.Validators
{
    public class ShipValidator
    {
        public static IEnumerable<string> Validate(Ship ship)
        {
            var totalWeight = GetShipWeight(ship);

            var weapons = ship.Wings.SelectMany(wing => wing.Hardpoint).ToList();
            var weaponsByType = weapons.GroupBy(weapon => weapon.DamageType);
            var weaponTypes = weapons.Select(weapon => weapon.DamageType).Distinct().ToList();

            double consumedEnergy = 0;
            foreach (var typeWeapons in weaponsByType)
            {
                double energy = typeWeapons.Sum(weapon => weapon.EnergyDrain);

                if (typeWeapons.Count() >= 3)
                {
                    energy *= 0.8;
                }

                consumedEnergy += energy;
            }

            #region Hull

            if (totalWeight > (int) ship.Hull.DefaultMaximumTakeOffMass)
            {
                yield return "The total weight of the Engine, Wings and Weapons exceeds the Hulls maximum take off mass.";
            }

            #endregion

            #region Engines

            if (consumedEnergy > ship.Energy)
            {
                yield return "The Engine cannot provide enough energy to power the Wings and Weapons.";
            }

            if (ship.Engine.Name == "Intrepid Class" && weapons.Any(weapon => weapon.Name == "Imploder"))
            {
                yield return "The Intrepid Class Engine cannot be used while using an Imploder Weapon.";
            }

            #endregion

            #region Wings

            if (ship.Wings.Count % 2 != 0)
            {
                yield return "The Ship must contain an even number of Wings.";
            }

            foreach (var wing in ship.Wings.Where(wing => wing.Hardpoint.Count > wing.NumberOfHardpoints))
            {
                yield return $"The {wing.Name} Wing contains too many Weapons. Maximum: {wing.NumberOfHardpoints}.";
            }

            #endregion

            #region Weapons

            if (weaponTypes.Contains(DamageTypeEnum.Heat) && weaponTypes.Contains(DamageTypeEnum.Cold))
            {
                yield return "The combination of a Heat Weapon and a Cold Weapon is not allowed.";
            }

            if (weaponTypes.Contains(DamageTypeEnum.Statis) && weaponTypes.Contains(DamageTypeEnum.Gravity))
            {
                yield return "The combination of a Statis Weapons and a Gravity Weapon is not allowed.";
            }

            //TODO (Het verschil in energieverbruik door Kinetic Weapons tussen de afzonderlijke vleugels moet kleiner dan 35 zijn)

            if (ship.Wings.Any(wing => wing.Hardpoint.Count == 1 && wing.Hardpoint.First().Name == "Nullifier"))
            {
                yield return "The Nullifier Weapon cannot be the only Weapon on a Wing.";
            }

            #endregion
        }

        public static double GetShipWeight(Ship ship)
        {
            double weight = ship.Engine.Weight + ship.Wings.Sum(wing => wing.Weight + wing.Hardpoint.Sum(weapon => weapon.Weight));

            if (ship.Wings.SelectMany(wing => wing.Hardpoint).Count(weapon => weapon.DamageType == DamageTypeEnum.Statis) >= 2)
            {
                weight *= 0.85; // Ik neem aan dat uitrustingstukken alle soorten zijn (Engine, Wings and Weapons)
            }

            return weight;
        }
    }
}