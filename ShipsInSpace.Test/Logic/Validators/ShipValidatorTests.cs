using System.Collections.Generic;
using System.Linq;
using GalacticSpaceTransitAuthority;
using ShipsInSpace.Logic.Extensions;
using ShipsInSpace.Logic.Licenses;
using ShipsInSpace.Logic.Validators;
using Xunit;

namespace ShipsInSpace.Test.Logic.Validators
{
    public class ShipValidatorTests
    {
        [Theory]
        [Trait("Category", "LicenseWeight")]
        [InlineData(PilotLicense.A)]
        [InlineData(PilotLicense.B)]
        [InlineData(PilotLicense.C)]
        public void LicenseWeight_TooMuch(PilotLicense license)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = license.GetMaxWeight() + 1
                },
                Wings = new List<Wing>()
            };

            Assert.False(ShipValidator.ValidLicense(ship, license));
        }

        [Theory]
        [Trait("Category", "LicenseWeight")]
        [InlineData(PilotLicense.A)]
        [InlineData(PilotLicense.B)]
        [InlineData(PilotLicense.C)]
        [InlineData(PilotLicense.Z)]
        public void LicenseWeight_Same(PilotLicense license)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = license.GetMaxWeight()
                },
                Wings = new List<Wing>()
            };

            Assert.True(ShipValidator.ValidLicense(ship, license));
        }

        [Theory]
        [Trait("Category", "LicenseWeight")]
        [InlineData(PilotLicense.A)]
        [InlineData(PilotLicense.B)]
        [InlineData(PilotLicense.C)]
        [InlineData(PilotLicense.Z)]
        public void LicenseWeight_Less(PilotLicense license)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = license.GetMaxWeight() - 1
                },
                Wings = new List<Wing>()
            };

            Assert.True(ShipValidator.ValidLicense(ship, license));
        }

        [Theory]
        [Trait("Category", "MaximumTakeOffMass")]
        [InlineData(TakeOffMassEnum.Interceptor)]
        [InlineData(TakeOffMassEnum.Tank)]
        [InlineData(TakeOffMassEnum.HeavyTank)]
        [InlineData(TakeOffMassEnum.LightFighter)]
        [InlineData(TakeOffMassEnum.MediumFighter)]
        public void MaximumTakeOffMass_TooMuch(TakeOffMassEnum takeOffMass)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = (int) takeOffMass + 1
                },
                Hull = new Hull
                {
                    DefaultMaximumTakeOffMass = takeOffMass
                },
                Wings = new List<Wing>()
            };

            Assert.False(ShipValidator.ValidMaximumTakeOffMass(ship));
        }

        [Theory]
        [Trait("Category", "MaximumTakeOffMass")]
        [InlineData(TakeOffMassEnum.Interceptor)]
        [InlineData(TakeOffMassEnum.Tank)]
        [InlineData(TakeOffMassEnum.HeavyTank)]
        [InlineData(TakeOffMassEnum.LightFighter)]
        [InlineData(TakeOffMassEnum.MediumFighter)]
        public void MaximumTakeOffMass_Same(TakeOffMassEnum takeOffMass)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = (int) takeOffMass
                },
                Hull = new Hull
                {
                    DefaultMaximumTakeOffMass = takeOffMass
                },
                Wings = new List<Wing>()
            };

            Assert.True(ShipValidator.ValidMaximumTakeOffMass(ship));
        }

        [Theory]
        [Trait("Category", "MaximumTakeOffMass")]
        [InlineData(TakeOffMassEnum.Interceptor)]
        [InlineData(TakeOffMassEnum.Tank)]
        [InlineData(TakeOffMassEnum.HeavyTank)]
        [InlineData(TakeOffMassEnum.LightFighter)]
        [InlineData(TakeOffMassEnum.MediumFighter)]
        public void MaximumTakeOffMass_Less(TakeOffMassEnum takeOffMass)
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Weight = (int) takeOffMass - 1
                },
                Hull = new Hull
                {
                    DefaultMaximumTakeOffMass = takeOffMass
                },
                Wings = new List<Wing>()
            };

            Assert.True(ShipValidator.ValidMaximumTakeOffMass(ship));
        }

        [Fact]
        [Trait("Category", "EnergyConsumption")]
        public void EnergyConsumption_TooMuch()
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Energy = 50
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        Energy = 50,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                EnergyDrain = 101
                            }
                        }
                    }
                }
            };

            Assert.False(ShipValidator.ValidEnergyConsumption(ship));
        }

        [Fact]
        [Trait("Category", "EnergyConsumption")]
        public void EnergyConsumption_Same()
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Energy = 50
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        Energy = 50,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                EnergyDrain = 100
                            }
                        }
                    }
                }
            };

            Assert.True(ShipValidator.ValidEnergyConsumption(ship));
        }

        [Fact]
        [Trait("Category", "EnergyConsumption")]
        public void EnergyConsumption_Less()
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Energy = 50
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        Energy = 50,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                EnergyDrain = 99
                            }
                        }
                    }
                }
            };

            Assert.True(ShipValidator.ValidEnergyConsumption(ship));
        }

        [Fact]
        [Trait("Category", "IntrepidImploder")]
        public void IntrepidImploder_WrongWeapon()
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Name = "Intrepid Class"
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Name = "Imploder"
                            }
                        }
                    }
                }
            };

            Assert.False(ShipValidator.ValidIntrepidImploder(ship));
        }

        [Fact]
        [Trait("Category", "IntrepidImploder")]
        public void IntrepidImploder_OtherWeapon()
        {
            var ship = new Ship
            {
                Engine = new Engine
                {
                    Name = "Intrepid Class"
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Name = "Hailstorm"
                            }
                        }
                    }
                }
            };

            Assert.True(ShipValidator.ValidIntrepidImploder(ship));
        }

        [Theory]
        [Trait("Category", "WingCount")]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(20)]
        [InlineData(100)]
        public void WingCount_RightCount(int wingCount)
        {
            var wings = new List<Wing>();

            for (var i = 0; i < wingCount; i++)
            {
                wings.Add(new Wing());
            }

            Assert.True(ShipValidator.ValidWingCount(wings));
        }

        [Theory]
        [Trait("Category", "WingCount")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(49)]
        [InlineData(99)]
        public void WingCount_WrongCount(int wingCount)
        {
            var wings = new List<Wing>();

            for (var i = 0; i < wingCount; i++)
            {
                wings.Add(new Wing());
            }

            Assert.False(ShipValidator.ValidWingCount(wings));
        }

        [Theory]
        [Trait("Category", "WingHardpointCount")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(8)]
        [InlineData(16)]
        public void WingHardpointCount_Same(int weaponCount)
        {
            var wing = new Wing
            {
                Hardpoint = new List<Weapon>(),
                NumberOfHardpoints = weaponCount
            };

            for (var i = 0; i < weaponCount; i++)
            {
                wing.Hardpoint.Add(new Weapon());
            }

            Assert.True(ShipValidator.ValidWingHardpointCount(wing));
        }

        [Theory]
        [Trait("Category", "WingHardpointCount")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(8)]
        [InlineData(16)]
        public void WingHardpointCount_TooMuch(int weaponCount)
        {
            var wing = new Wing
            {
                Hardpoint = new List<Weapon>(),
                NumberOfHardpoints = weaponCount - 1
            };

            for (var i = 0; i < weaponCount; i++)
            {
                wing.Hardpoint.Add(new Weapon());
            }

            Assert.False(ShipValidator.ValidWingHardpointCount(wing));
        }

        [Fact]
        [Trait("Category", "HeatColdCombination")]
        public void HeatColdCombination_HeatCold()
        {
            var weapons = new List<Weapon>
            {
                new()
                {
                    DamageType = DamageTypeEnum.Heat
                },
                new()
                {
                    DamageType = DamageTypeEnum.Cold
                }
            };

            Assert.False(ShipValidator.ValidHeatColdCombination(weapons));
        }

        [Fact]
        [Trait("Category", "HeatColdCombination")]
        public void HeatColdCombination_HeatOther()
        {
            var weapons = new List<Weapon>
            {
                new()
                {
                    DamageType = DamageTypeEnum.Heat
                },
                new()
                {
                    DamageType = DamageTypeEnum.Kinetic
                }
            };

            Assert.True(ShipValidator.ValidHeatColdCombination(weapons));
        }

        [Fact]
        [Trait("Category", "HeatColdCombination")]
        public void HeatColdCombination_ColdOther()
        {
            var weapons = new List<Weapon>
            {
                new()
                {
                    DamageType = DamageTypeEnum.Kinetic
                },
                new()
                {
                    DamageType = DamageTypeEnum.Cold
                }
            };

            Assert.True(ShipValidator.ValidHeatColdCombination(weapons));
        }

        [Fact]
        [Trait("Category", "HeatColdCombination")]
        public void HeatColdCombination_Other()
        {
            var weapons = new List<Weapon>
            {
                new()
                {
                    DamageType = DamageTypeEnum.Kinetic
                },
                new()
                {
                    DamageType = DamageTypeEnum.Kinetic
                }
            };

            Assert.True(ShipValidator.ValidHeatColdCombination(weapons));
        }

        [Fact]
        [Trait("Category", "StatisGravityCombination")]
        public void StatisGravityCombination_StatisGravity()
        {
            var weapons = new List<Weapon>
            {
                new()
                {
                    DamageType = DamageTypeEnum.Statis
                },
                new()
                {
                    DamageType = DamageTypeEnum.Gravity
                }
            };

            Assert.False(ShipValidator.ValidStatisGravityCombination(weapons));
        }

        [Fact]
        [Trait("Category", "StatisGravityCombination")]
        public void StatisGravityCombination_StatisOther()
        {
            var weapons = new List<Weapon>
            {
                new()
                {
                    DamageType = DamageTypeEnum.Statis
                },
                new()
                {
                    DamageType = DamageTypeEnum.Kinetic
                }
            };

            Assert.True(ShipValidator.ValidStatisGravityCombination(weapons));
        }

        [Fact]
        [Trait("Category", "StatisGravityCombination")]
        public void StatisGravityCombination_ColdOther()
        {
            var weapons = new List<Weapon>
            {
                new()
                {
                    DamageType = DamageTypeEnum.Kinetic
                },
                new()
                {
                    DamageType = DamageTypeEnum.Gravity
                }
            };

            Assert.True(ShipValidator.ValidStatisGravityCombination(weapons));
        }

        [Fact]
        [Trait("Category", "StatisGravityCombination")]
        public void StatisGravityCombination_Other()
        {
            var weapons = new List<Weapon>
            {
                new()
                {
                    DamageType = DamageTypeEnum.Kinetic
                },
                new()
                {
                    DamageType = DamageTypeEnum.Kinetic
                }
            };

            Assert.True(ShipValidator.ValidStatisGravityCombination(weapons));
        }

        [Theory]
        [Trait("Category", "KineticWings")]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(40)]
        [InlineData(60)]
        [InlineData(80)]
        [InlineData(100)]
        public void KineticWings_One(int energyDrain)
        {
            var wings = new List<Wing>
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
            };

            Assert.True(ShipValidator.ValidKineticWings(wings));
        }

        [Theory]
        [Trait("Category", "KineticWings")]
        [InlineData(0)]
        [InlineData(20)]
        [InlineData(40)]
        [InlineData(60)]
        [InlineData(80)]
        [InlineData(100)]
        public void KineticWings_Multiple_Same(int energyDrain)
        {
            var wings = new List<Wing>
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
                },
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
            };

            Assert.True(ShipValidator.ValidKineticWings(wings));
        }

        [Theory]
        [Trait("Category", "KineticWings")]
        [InlineData(0, 20)]
        [InlineData(20, 17)]
        [InlineData(40, 14)]
        [InlineData(60, 83)]
        [InlineData(80, 58)]
        [InlineData(100, 124)]
        public void KineticWings_Multiple_Difference_Right(int energyDrain1, int energyDrain2)
        {
            var wings = new List<Wing>
            {
                new()
                {
                    Hardpoint = new List<Weapon>
                    {
                        new()
                        {
                            EnergyDrain = energyDrain1
                        }
                    }
                },
                new()
                {
                    Hardpoint = new List<Weapon>
                    {
                        new()
                        {
                            EnergyDrain = energyDrain2
                        }
                    }
                }
            };

            Assert.True(ShipValidator.ValidKineticWings(wings));
        }

        [Theory]
        [Trait("Category", "KineticWings")]
        [InlineData(0, 38)]
        [InlineData(20, 84)]
        [InlineData(40, 2)]
        [InlineData(60, 23)]
        [InlineData(80, 39)]
        [InlineData(100, 158)]
        public void KineticWings_Multiple_Difference_TooMuch(int energyDrain1, int energyDrain2)
        {
            var wings = new List<Wing>
            {
                new()
                {
                    Hardpoint = new List<Weapon>
                    {
                        new()
                        {
                            EnergyDrain = energyDrain1
                        }
                    }
                },
                new()
                {
                    Hardpoint = new List<Weapon>
                    {
                        new()
                        {
                            EnergyDrain = energyDrain2
                        }
                    }
                }
            };

            Assert.False(ShipValidator.ValidKineticWings(wings));
        }

        [Fact]
        [Trait("Category", "NullifierCount")]
        public void NullifierCount_One_Mullifier()
        {
            var wing = new Wing
            {
                Hardpoint = new List<Weapon>
                {
                    new()
                    {
                        Name = "Nullifier"
                    }
                }
            };

            Assert.False(ShipValidator.ValidNullifierCount(wing));
        }

        [Fact]
        [Trait("Category", "NullifierCount")]
        public void NullifierCount_One_Other()
        {
            var wing = new Wing
            {
                Hardpoint = new List<Weapon>
                {
                    new()
                    {
                        Name = "Fury Cannon"
                    }
                }
            };

            Assert.True(ShipValidator.ValidNullifierCount(wing));
        }

        [Theory]
        [Trait("Category", "NullifierCount")]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(8)]
        public void NullifierCount_Multiple_One_Mullifier(int weaponCount)
        {
            var wing = new Wing
            {
                Hardpoint = new List<Weapon>
                {
                    new()
                    {
                        Name = "Mullifier"
                    }
                }
            };

            for (var i = 0; i < weaponCount; i++)
            {
                wing.Hardpoint.Add(new Weapon
                {
                    Name = "Hailstorm"
                });
            }

            Assert.True(ShipValidator.ValidNullifierCount(wing));
        }

        [Theory]
        [Trait("Category", "NullifierCount")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(8)]
        public void NullifierCount_Multiple_No_Mullifier(int weaponCount)
        {
            var wing = new Wing
            {
                Hardpoint = new List<Weapon>()
            };

            for (var i = 0; i < weaponCount; i++)
            {
                wing.Hardpoint.Add(new Weapon
                {
                    Name = "Hailstorm"
                });
            }

            Assert.True(ShipValidator.ValidNullifierCount(wing));
        }

        [Theory]
        [Trait("Category", "NullifierCount")]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(8)]
        public void NullifierCount_Multiple_Multiple_Mullifier(int weaponCount)
        {
            var wing = new Wing
            {
                Hardpoint = new List<Weapon>()
            };

            for (var i = 0; i < weaponCount; i++)
            {
                wing.Hardpoint.Add(new Weapon
                {
                    Name = "Mullifier"
                });
            }

            Assert.True(ShipValidator.ValidNullifierCount(wing));
        }

        [Fact]
        public void Validate_Right()
        {
            var ship = new Ship
            {
                Hull = new Hull
                {
                    DefaultMaximumTakeOffMass = TakeOffMassEnum.HeavyTank
                },
                Engine = new Engine
                {
                    Energy = 400,
                    Weight = 100
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        NumberOfHardpoints = 2,
                        Energy = 100,
                        Weight = 50,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Weight = 25,
                                Name = "Shockwave",
                                DamageType = DamageTypeEnum.Kinetic
                            }
                        }
                    },
                    new()
                    {
                        NumberOfHardpoints = 4,
                        Energy = 100,
                        Weight = 50,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Weight = 25,
                                Name = "Nullifier",
                                DamageType = DamageTypeEnum.Gravity
                            },
                            new()
                            {
                                Weight = 25,
                                Name = "Shockwave",
                                DamageType = DamageTypeEnum.Heat
                            }
                        }
                    }
                }
            };

            Assert.Empty(ShipValidator.Validate(ship, PilotLicense.Z));
        }

        [Fact]
        public void Validate_Wrong()
        {
            var ship = new Ship
            {
                Hull = new Hull
                {
                    DefaultMaximumTakeOffMass = TakeOffMassEnum.Interceptor
                },
                Engine = new Engine
                {
                    Name = "Intrepid Class",
                    Energy = 30,
                    Weight = 500
                },
                Wings = new List<Wing>
                {
                    new()
                    {
                        NumberOfHardpoints = 2,
                        Energy = 10,
                        Weight = 250,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Weight = 75,
                                EnergyDrain = 100,
                                Name = "Imploder",
                                DamageType = DamageTypeEnum.Cold
                            },
                            new()
                            {
                                Weight = 75,
                                EnergyDrain = 100,
                                Name = "Imploder",
                                DamageType = DamageTypeEnum.Kinetic
                            }
                        }
                    },
                    new()
                    {
                        NumberOfHardpoints = 1,
                        Energy = 10,
                        Weight = 250,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Weight = 75,
                                EnergyDrain = 100,
                                Name = "Shockwave",
                                DamageType = DamageTypeEnum.Statis
                            },
                            new()
                            {
                                Weight = 75,
                                EnergyDrain = 100,
                                Name = "Shockwave",
                                DamageType = DamageTypeEnum.Heat
                            },
                            new()
                            {
                                Weight = 75,
                                EnergyDrain = 30,
                                Name = "Imploder",
                                DamageType = DamageTypeEnum.Kinetic
                            }
                        }
                    },
                    new()
                    {
                        NumberOfHardpoints = 2,
                        Energy = 10,
                        Weight = 250,
                        Hardpoint = new List<Weapon>
                        {
                            new()
                            {
                                Weight = 75,
                                EnergyDrain = 100,
                                Name = "Nullifier",
                                DamageType = DamageTypeEnum.Gravity
                            }
                        }
                    }
                }
            };

            Assert.Equal(10, ShipValidator.Validate(ship, PilotLicense.A).Distinct().Count());
        }
    }
}