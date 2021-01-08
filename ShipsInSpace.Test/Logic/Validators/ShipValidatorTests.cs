using System.Collections.Generic;
using System.ComponentModel;
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
    }
}