using System.Collections.Generic;
using System.Linq;
using GalacticSpaceTransitAuthority;
using Moq;
using ShipsInSpace.Logic;
using ShipsInSpace.Logic.Extensions;
using Xunit;

namespace ShipsInSpace.Test.Logic.Builders
{
    public class ShipBuilderTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(34)]
        [InlineData(54)]
        [InlineData(56)]
        public void Id_Change(int shipId)
        {
            var builder = new ShipBuilder(null);
            builder.SetId(shipId);

            Assert.Equal(shipId, builder.Build().Id);
        }


        [Theory]
        [InlineData("Rachid")]
        [InlineData("Ali B")]
        [InlineData("Sander Jochems")]
        [InlineData("Tommy Hosewol")]
        public void Name_Change(string shipName)
        {
            var builder = new ShipBuilder(null);
            builder.SetName(shipName);

            Assert.Equal(shipName, builder.Build().Name);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        public void Hull_Change(int hullId)
        {
            var authority = new Mock<ISpaceTransitAuthority>();

            authority.Setup(transitAuthority => transitAuthority.GetHulls()).Returns(new List<Hull>
            {
                new()
                {
                    Id = 1
                },
                new()
                {
                    Id = 2
                },
                new()
                {
                    Id = 3
                },
                new()
                {
                    Id = 4
                },
                new()
                {
                    Id = 5
                }
            });

            var builder = new ShipBuilder(authority.Object);
            builder.SetHull(hullId);

            Assert.Equal(hullId, builder.Build().Hull.Id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        public void Engine_Change(int engineId)
        {
            var authority = new Mock<ISpaceTransitAuthority>();

            authority.Setup(transitAuthority => transitAuthority.GetEngines()).Returns(new List<Engine>
            {
                new()
                {
                    Id = 1
                },
                new()
                {
                    Id = 2
                },
                new()
                {
                    Id = 3
                },
                new()
                {
                    Id = 4
                },
                new()
                {
                    Id = 5
                }
            });

            var builder = new ShipBuilder(authority.Object);
            builder.SetEngine(engineId);

            Assert.Equal(engineId, builder.Build().Engine.Id);
        }

        [Theory]
        [InlineData(1, new int[] {1, 2})]
        [InlineData(2, new int[] {2, 3})]
        [InlineData(3, new int[] {4, 1})]
        [InlineData(5, new int[] {2, 2})]
        public void Wing_Add(int wingId, int[] weaponsIds)
        {
            var authority = new Mock<ISpaceTransitAuthority>();
            authority.Setup(transitAuthority => transitAuthority.GetWings()).Returns(new List<Wing>
            {
                new()
                {
                    Id = 1
                },
                new()
                {
                    Id = 2
                },
                new()
                {
                    Id = 3
                },
                new()
                {
                    Id = 4
                },
                new()
                {
                    Id = 5
                }
            });

            authority.Setup(transitAuthority => transitAuthority.GetWeapons()).Returns(new List<Weapon>
            {
                new()
                {
                    Id = 1
                },
                new()
                {
                    Id = 2
                },
                new()
                {
                    Id = 3
                },
                new()
                {
                    Id = 4
                },
                new()
                {
                    Id = 5
                }
            });

            var pairs = new List<KeyValuePair<int, int[]>>
            {
                new(wingId, weaponsIds)
            };

            var builder = new ShipBuilder(authority.Object);
            builder.AddWing(pairs);

            var ship = builder.Build();

            Assert.Single(ship.Wings);
            foreach (var weaponId in weaponsIds)
            {
                Assert.Contains(ship.Wings, wing => wing.Hardpoint.Any(weapon => weapon.Id == weaponId));
            }
        }
    }
}