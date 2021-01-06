﻿using System.Collections.Generic;
using System.Linq;
using GalacticSpaceTransitAuthority;

namespace ShipsInSpace.Logic.Extensions
{
    public static class ShipExtensions
    {
        public static double GetWeight(this Ship ship)
        {
            double weight = ship.Engine.Weight + ship.Wings.Sum(wing => wing.Weight + wing.Hardpoint.Sum(weapon => weapon.Weight));

            if (ship.Wings.SelectMany(wing => wing.Hardpoint).Count(weapon => weapon.DamageType == DamageTypeEnum.Statis) >= 2)
            {
                weight *= 0.85; // Ik neem aan dat uitrustingstukken alle soorten zijn (Engine, Wings and Weapons)
            }

            return weight;
        }

        public static double GetEnergyConsumption(this Ship ship)
        {
            var weaponsByType = GetWeapons(ship).GroupBy(weapon => weapon.DamageType);

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

        public static IEnumerable<Weapon> GetWeapons(this Ship ship)
        {
            return ship.Wings.SelectMany(wing => wing.Hardpoint);
        }
    }
}