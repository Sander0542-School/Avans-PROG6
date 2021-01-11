using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShipsInSpace.Logic.Generators;
using ShipsInSpace.Logic.Helpers;
using ShipsInSpace.Web.Models.Pirates;

namespace ShipsInSpace.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class PiratesController : Controller
    {
        private readonly SecretKeyGenerator _secretKeyGenerator;
        private readonly UserHelper _userHelper;
        private readonly UserManager<IdentityUser> _userManager;

        public PiratesController(UserManager<IdentityUser> userManager, SecretKeyGenerator secretKeyGenerator, UserHelper userHelper)
        {
            _userManager = userManager;
            _secretKeyGenerator = secretKeyGenerator;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            var pirates = await _userManager.GetUsersInRoleAsync("Pirate");

            var pirateModels = new List<PirateModel>();

            foreach (var pirate in pirates)
            {
                var license = await _userHelper.GetLicense(pirate);

                pirateModels.Add(new PirateModel
                {
                    Id = pirate.Id,
                    LicensePlate = pirate.UserName,
                    PilotLicense = license
                });
            }

            return View(pirateModels);
        }

        public async Task<IActionResult> Create()
        {
            return View(new CreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var secretKey = _secretKeyGenerator.Generate();

                var user = new IdentityUser
                {
                    UserName = model.LicensePlate
                };

                var result = await _userManager.CreateAsync(user, secretKey);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim("License", model.PilotLicense.ToString()));
                    await _userManager.AddToRoleAsync(user, "Pirate");

                    TempData["SecretKey"] = secretKey;
                    return RedirectToAction(nameof(Letter), new {userId = user.Id});
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Letter(string userId)
        {
            if (userId == null) return NotFound();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();
            if (!await _userManager.IsInRoleAsync(user, "Pirate")) return NotFound();

            var license = await _userHelper.GetLicense(user);

            var letterViewModel = new LetterViewModel
            {
                Id = user.Id,
                LicensePlate = user.UserName,
                PilotLicense = license,
                SecretKey = TempData["SecretKey"].ToString()
            };

            return View(letterViewModel);
        }
    }
}