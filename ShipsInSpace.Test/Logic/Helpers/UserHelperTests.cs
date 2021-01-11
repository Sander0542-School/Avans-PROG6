using System;
using System.Collections.Generic;
using System.Security.Claims;
using ShipsInSpace.Logic.Helpers;
using ShipsInSpace.Logic.Licenses;
using Xunit;

namespace ShipsInSpace.Test.Logic.Helpers
{
    public class UserHelperTests
    {
        [Theory]
        [InlineData(PilotLicense.A)]
        [InlineData(PilotLicense.B)]
        [InlineData(PilotLicense.C)]
        [InlineData(PilotLicense.Z)]
        public async void GetLicenseTest(PilotLicense license)
        {
            var userManager = new UserHelper(null);

            var claims = new List<Claim>
            {
                new("License", license.ToString())
            };

            Assert.Equal(license, await userManager.GetLicense(claims));
        }

        // private UserHelper CreateUserHelper()
        // {
        //     return new UserHelper(CreateUserManager());
        // }
        //
        // private UserManager<IdentityUser> CreateUserManager()
        // {
        //     var userManager = new Mock<UserManager<IdentityUser>>();
        //
        //     var user = new IdentityUser
        //     {
        //         Id = Guid.NewGuid().ToString(),
        //         UserName = "John the Tester",
        //         Email = "john@shipinspace.test"
        //     };
        //
        //     var claims = new List<Claim>
        //     {
        //         new("License", "3")
        //     };
        //
        //     userManager.Setup(userManager1 => userManager1.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        //     userManager.Setup(userManager1 => userManager1.GetClaimsAsync(It.IsAny<IdentityUser>())).ReturnsAsync(claims);
        //
        //     return userManager.Object;
        // }
    }
}