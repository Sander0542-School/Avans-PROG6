using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GalacticSpaceTransitAuthority;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShipsInSpace.Logic;
using ShipsInSpace.Logic.Helpers;
using ShipsInSpace.Logic.Validators;
using ShipsInSpace.Web.Models.Ship;

namespace ShipsInSpace.Web.Controllers
{
    [Authorize(Roles = "Pirate")]
    public class ShipController : Controller
    {
        private readonly ShipBuilder _shipBuilder;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ISpaceTransitAuthority _transitAuthority;
        private readonly UserHelper _userHelper;
        private readonly UserManager<IdentityUser> _userManager;

        public ShipController(UserManager<IdentityUser> userManager, ISpaceTransitAuthority transitAuthority,
            ShipBuilder shipBuilder, UserHelper userHelper, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _transitAuthority = transitAuthority;
            _shipBuilder = shipBuilder;
            _userHelper = userHelper;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(HullEngine));
        }

        [HttpGet]
        public IActionResult HullEngine()
        {
            return View(BuildHullEngineViewModel());
        }

        [HttpPost]
        public IActionResult HullEngine(HullEngineViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["Input.HullEngine"] = JsonSerializer.Serialize(model.Input);
                return RedirectToAction(nameof(Wings));
            }

            return View(BuildHullEngineViewModel(model.Input));
        }

        [HttpGet]
        public IActionResult Wings()
        {
            if (TempData.ContainsKey("Input.HullEngine"))
            {
                var hullEngineInput =
                    JsonSerializer.Deserialize<HullEngineViewModel.InputModel>(TempData["Input.HullEngine"] as string);

                return View(BuildWingsViewModel(hullEngineInput));
            }

            return RedirectToAction(nameof(HullEngine));
        }

        [HttpPost]
        public async Task<IActionResult> Wings(WingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var license = await _userHelper.GetLicense(User.Claims);

                var ship = _shipBuilder.SetHull(model.Input.HullId)
                    .SetEngine(model.Input.EngineId)
                    .AddWing(model.Input.Wings.Select(wing => new KeyValuePair<int, int[]>(wing.WingId, wing.Weapons)))
                    .Build();

                var errors = ShipValidator.Validate(ship, license).ToList();

                if (!errors.Any())
                {
                    TempData["Input.HullEngineWings"] = JsonSerializer.Serialize(model.Input);
                    return RedirectToAction(nameof(Confirm));
                }

                foreach (var error in errors) ModelState.AddModelError("Ship", error);
            }

            return View(BuildWingsViewModel(model.Input));
        }

        [HttpGet]
        public async Task<IActionResult> Confirm()
        {
            if (TempData.ContainsKey("Input.HullEngineWings"))
            {
                var hullEngineWingsInput =
                    JsonSerializer.Deserialize<WingsViewModel.InputModel>(TempData["Input.HullEngineWings"] as string);

                var ship = _shipBuilder.SetName(await _userHelper.GetUserName(User))
                    .SetHull(hullEngineWingsInput.HullId)
                    .SetEngine(hullEngineWingsInput.EngineId)
                    .AddWing(hullEngineWingsInput.Wings.Select(wing =>
                        new KeyValuePair<int, int[]>(wing.WingId, wing.Weapons)))
                    .Build();

                var model = new ConfirmViewModel
                {
                    Ship = ship,
                    Input = hullEngineWingsInput
                };

                return View(model);
            }

            return RedirectToAction(nameof(HullEngine));
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ship = _shipBuilder.SetName("Temp")
                    .SetHull(model.Input.HullId)
                    .SetEngine(model.Input.EngineId)
                    .AddWing(model.Input.Wings.Select(wing => new KeyValuePair<int, int[]>(wing.WingId, wing.Weapons)))
                    .Build();

                var errors = ShipValidator.Validate(ship, await _userHelper.GetLicense(User.Claims));
                if (!errors.Any())
                {
                    var jsonShip = JsonSerializer.Serialize(ship);

                    var shipId = _transitAuthority.RegisterShip(jsonShip);

                    if (!string.IsNullOrWhiteSpace(shipId)) return RedirectToAction(nameof(Registered), new {shipId});
                }
            }

            TempData["Input.HullEngineWings"] = JsonSerializer.Serialize(model.Input);
            return RedirectToAction(nameof(Wings));
        }

        public async Task<IActionResult> Registered(string shipId)
        {
            var user = await _userManager.GetUserAsync(User);
            await _signInManager.SignOutAsync();
            await _userManager.DeleteAsync(user);

            return View("Registered", shipId);
        }

        #region ApiHelpers

        #region HullEngine

        private HullEngineViewModel BuildHullEngineViewModel()
        {
            return new()
            {
                Hulls = _transitAuthority.GetHulls(),
                Engines = _transitAuthority.GetEngines(),
                Input = new HullEngineViewModel.InputModel()
            };
        }

        private HullEngineViewModel BuildHullEngineViewModel(HullEngineViewModel.InputModel inputModel)
        {
            var model = BuildHullEngineViewModel();

            model.Input = inputModel;

            return model;
        }

        #endregion

        #region Wings

        private WingsViewModel BuildWingsViewModel()
        {
            return new()
            {
                Wings = _transitAuthority.GetWings(),
                Weapons = _transitAuthority.GetWeapons(),
                Input = new WingsViewModel.InputModel()
            };
        }

        private WingsViewModel BuildWingsViewModel(HullEngineViewModel.InputModel inputModel)
        {
            var model = BuildWingsViewModel();

            model.Input = new WingsViewModel.InputModel
            {
                HullId = inputModel.HullId,
                EngineId = inputModel.EngineId,
                WingCount = inputModel.WingCount
            };

            return model;
        }

        private WingsViewModel BuildWingsViewModel(WingsViewModel.InputModel inputModel)
        {
            var model = BuildWingsViewModel();

            model.Input = inputModel;

            return model;
        }

        #endregion

        #endregion
    }
}