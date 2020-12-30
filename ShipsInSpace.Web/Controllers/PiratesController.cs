using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShipsInSpace.Data.Models;
using ShipsInSpace.Logic.Generators;
using ShipsInSpace.Web.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShipsInSpace.Web.Controllers
{

    [Authorize(Roles = "Manager")]
    public class PiratesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SecretKeyGenerator _secretKeyGenerator;

        public PiratesController(UserManager<User> userManager, SecretKeyGenerator secretKeyGenerator)
        {
            _userManager = userManager;
            _secretKeyGenerator = secretKeyGenerator;
        }

        public async Task<IActionResult> Index()
        {
            var pirates = await _userManager.GetUsersInRoleAsync("Pirate");

            var model = pirates.Select(user => new PirateModel
            {
                Id = user.Id,
                LicensePlate = user.UserName,
                PilotLicense = user.PilotLicense
            });

            return View(model);
        }

        public async Task<IActionResult> Create() => View(new CreateViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var secretKey = _secretKeyGenerator.Generate();

                var user = new User
                {
                    UserName = model.LicensePlate,
                    Email = $"{model.LicensePlate}@galaxy.space",
                    PilotLicense = model.PilotLicense,
                    SecretKey = secretKey
                };

                var result = await _userManager.CreateAsync(user, secretKey);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Pirate");
                    
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Letter(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();
            if (!await _userManager.IsInRoleAsync(user, "Pirate")) return NotFound();

            var letterViewModel = new LetterViewModel
            {
                Id = user.Id,
                LicensePlate = user.UserName,
                PilotLicense = user.PilotLicense,
                SecretKey = user.SecretKey
            };

            return View(letterViewModel);
        }
    }
}
