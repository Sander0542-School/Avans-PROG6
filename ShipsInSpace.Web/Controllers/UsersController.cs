using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShipsInSpace.Data.Models;
using ShipsInSpace.Web.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShipsInSpace.Web.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();

            var usersInModel = users.Select(user => new ViewModel
            {
                Id = user.Id,
                LicensePlate = user.UserName,
                PilotLicense = user.PilotLicense
            });

            return View(usersInModel);
        }

        public async Task<IActionResult> Create()
        {
           // User user = await _userManager.GetUserAsync(User);

            return View(new CreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                string secretKey = new string(Enumerable.Repeat(characters, 8).Select(s => s[random.Next(s.Length)]).ToArray());

                User user = new User
                {
                    UserName = createViewModel.LicensePlate,
                    Email = $"{createViewModel.LicensePlate}@galaxy.space",
                    PilotLicense = createViewModel.PilotLicense,
                    SecretKey = secretKey
                };

                var result = await _userManager.CreateAsync(user, secretKey);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(createViewModel);
        }

        public async Task<IActionResult> Letter(string id)
        {
            if (id == null) return NotFound();

            User user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            LetterViewModel letterViewModel = new LetterViewModel
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
