using System.Collections.Generic;
using System.Linq;
using GalacticSpaceTransitAuthority;
using ShipsInSpace.Logic.Extensions;
using Xunit;

namespace ShipsInSpace.Test.Logic.Extensions
{
    public class ShipExtensionsTests
    {
        [Theory]
        [Trait("Category", "Weight")]
        [InlineData(20)]
        [InlineData(33)]
        [InlineData(48)]
        [InlineData(64)]
        [InlineData(200)]
        public void Weight_Engine(int engineWeight)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = engineWeight
                },
                Wings = new List<Wing>()
            };

            Assert.Equal(engineWeight, ship.GetWeight());
        }

        [Theory]
        [Trait("Category", "Weight")]
        [InlineData(20)]
        [InlineData(33)]
        [InlineData(48)]
        [InlineData(64)]
        [InlineData(200)]
        public void Weight_Wings(int wingWeight)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = 0
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        Weight = wingWeight,
                        Hardpoint = new List<Weapon>()
                    },
                    new()
                    {
                        Weight = wingWeight,
                        Hardpoint = new List<Weapon>()
                    }
                }
            };

            var expectedWeight = wingWeight * ship.Wings.Count;

            Assert.Equal(expectedWeight, ship.GetWeight());
        }

        [Theory]
        [Trait("Category", "Weight")]
        [InlineData(20)]
        [InlineData(33)]
        [InlineData(48)]
        [InlineData(64)]
        [InlineData(200)]
        public void Weight_Weapons(int weaponWeight)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = 0
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        Weight = 0,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Weight = weaponWeight
                            },
                            new()
                            {
                                Weight = weaponWeight
                            }
                        }
                    },
                    new()
                    {
                        Weight = 0,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Weight = weaponWeight
                            }
                        }
                    }
                }
            };

            var expectedWeight = weaponWeight * ship.Wings.SelectMany(wing => wing.Hardpoint).Count();

            Assert.Equal(expectedWeight, ship.GetWeight());
        }

        [Theory]
        [Trait("Category", "Weight")]
        [InlineData(20)]
        [InlineData(33)]
        [InlineData(48)]
        [InlineData(64)]
        [InlineData(200)]
        public void Weight_Weapons_Statis(int weaponWeight)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = 0
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        Weight = 0,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Weight = weaponWeight,
                                DamageType = DamageTypeEnum.Statis
                            },
                            new()
                            {
                                Weight = weaponWeight,
                                DamageType = DamageTypeEnum.Statis
                            }
                        }
                    },
                    new()
                    {
                        Weight = 0,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Weight = weaponWeight,
                                DamageType = DamageTypeEnum.Statis
                            }
                        }
                    }
                }
            };

            var expectedWeight = weaponWeight * (1 - ShipExtensions.StatisWeaponReduction);
            expectedWeight *= ship.Wings.SelectMany(wing => wing.Hardpoint).Count();

            Assert.Equal((int)expectedWeight, ship.GetWeight());
        }

        [Theory]
        [Trait("Category", "EnergyConsumption")]
        [InlineData(20)]
        [InlineData(33)]
        [InlineData(48)]
        [InlineData(64)]
        [InlineData(200)]
        public void EnergyConsumption_Weapons_One(int energyDrain)
        {
            var ship = new Ship
            {
                Wings = new List<Wing>
                {
                    new()
                    {
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                EnergyDrain = energyDrain
                            }
                        }
                    }
                }
            };

            Assert.Equal(energyDrain, ship.GetEnergyConsumption());
        }

        [Theory]
        [Trait("Category", "EnergyConsumption")]
        [InlineData(20)]
        [InlineData(33)]
        [InlineData(48)]
        [InlineData(64)]
        [InlineData(200)]
        public void EnergyConsumption_Weapons_Multiple_SameType(int energyDrain)
        {
            var ship = new Ship
            {
                Wings = new List<Wing>
                {
                    new()
                    {
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                EnergyDrain = energyDrain,
                                DamageType = DamageTypeEnum.Cold
                            },
                            new()
                            {
                                EnergyDrain = energyDrain,
                                DamageType = DamageTypeEnum.Cold
                            },
                            new()
                            {
                                EnergyDrain = energyDrain,
                                DamageType = DamageTypeEnum.Cold
                            }
                        }
                    }
                }
            };

            var expectedEnergyDrain = energyDrain * (1 - ShipExtensions.WeaponEnergyReduction);
            expectedEnergyDrain *= ship.Wings.SelectMany(wing => wing.Hardpoint).Count();

            Assert.Equal((int) expectedEnergyDrain, ship.GetEnergyConsumption());
        }

        [Theory]
        [Trait("Category", "EnergyConsumption")]
        [InlineData(20)]
        [InlineData(33)]
        [InlineData(48)]
        [InlineData(64)]
        [InlineData(200)]
        public void EnergyConsumption_Weapons_Multiple_DifferentType(int energyDrain)
        {
            var ship = new Ship
            {
                Wings = new List<Wing>
                {
                    new()
                    {
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                EnergyDrain = energyDrain,
                                DamageType = DamageTypeEnum.Heat
                            },
                            new()
                            {
                                EnergyDrain = energyDrain,
                                DamageType = DamageTypeEnum.Cold
                            },
                            new()
                            {
                                EnergyDrain = energyDrain,
                                DamageType = DamageTypeEnum.Statis
                            }
                        }
                    }
                }
            };

            var expectedEnergyDrain = energyDrain * ship.Wings.SelectMany(wing => wing.Hardpoint).Count();

            Assert.Equal(expectedEnergyDrain, ship.GetEnergyConsumption());
        }
    }
}