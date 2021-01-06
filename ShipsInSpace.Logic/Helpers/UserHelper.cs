using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ShipsInSpace.Logic.Licenses;

namespace ShipsInSpace.Logic.Helpers
{
    public class UserHelper
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserHelper(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PilotLicense> GetLicense(IdentityUser user)
        {
            return await GetLicense(await _userManager.GetClaimsAsync(user));
        }

        public async Task<PilotLicense> GetLicense(IEnumerable<Claim> claims)
        {
            return Enum.Parse<PilotLicense>(claims.First(claim => claim.Type == "License").Value);
        }

        public async Task<string> GetUserName(ClaimsPrincipal user)
        {
            return (await _userManager.GetUserAsync(user)).UserName;
        }
    }
}