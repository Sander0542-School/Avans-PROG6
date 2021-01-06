using System.Collections.Generic;
using System.Linq;
using GalacticSpaceTransitAuthority;

namespace ShipsInSpace.Logic
{
    public class ShipBuilder
    {
        private readonly ISpaceTransitAuthority _transitAuthority;
        private readonly Ship _ship;

        public ShipBuilder(ISpaceTransitAuthority transitAuthority)
        {
            _transitAuthority = transitAuthority;

            _ship = new Ship
            {
                Wings = new List<Wing>()
            };
        }

        #region Id

        public ShipBuilder SetId(int id)
        {
            _ship.Id = id;

            return this;
        }

        #endregion

        #region Name

        public ShipBuilder SetName(string name)
        {
            _ship.Name = name;

            return this;
        }

        #endregion

        #region Hull

        public ShipBuilder SetHull(int hullId)
        {
            return SetHull(_transitAuthority.GetHulls().First(hull => hull.Id == hullId));
        }

        public ShipBuilder SetHull(Hull hull)
        {
            _ship.Hull = hull;

            return this;
        }

        #endregion

        #region Engine

        public ShipBuilder SetEngine(int engineId)
        {
            return SetEngine(_transitAuthority.GetEngines().First(hull => hull.Id == engineId));
        }

        public ShipBuilder SetEngine(Engine engine)
        {
            _ship.Engine = engine;

            return this;
        }

        #endregion

        #region Wings

        public ShipBuilder AddWing(IEnumerable<KeyValuePair<int, int[]>> wingWeapons)
        {
            foreach (var (wingId, weaponIds) in wingWeapons)
            {
                AddWing(wingId, weaponIds);
            }

            return this;
        }

        public ShipBuilder AddWing(int wingId, int[] weaponIds)
        {
            var wing = _transitAuthority.GetWings().First(wing1 => wing1.Id == wingId);
            wing.Hardpoint = weaponIds.Select(weaponId => _transitAuthority.GetWeapons().First(weapon => weapon.Id == weaponId)).ToList();

            return AddWing(wing);
        }

        public ShipBuilder AddWing(Wing wing)
        {
            _ship.Wings.Add(wing);

            return this;
        }

        #endregion

        public Ship Build()
        {
            return _ship;
        }
    }
}