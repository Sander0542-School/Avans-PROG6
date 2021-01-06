using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GalacticSpaceTransitAuthority;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ISpaceTransitAuthority _transitAuthority;
        private readonly ShipBuilder _shipBuilder;
        private readonly UserHelper _userHelper;

        public ShipController(ISpaceTransitAuthority transitAuthority, ShipBuilder shipBuilder, UserHelper userHelper)
        {
            _transitAuthority = transitAuthority;
            _shipBuilder = shipBuilder;
            _userHelper = userHelper;
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
                var hullEngineInput = JsonSerializer.Deserialize<HullEngineViewModel.InputModel>(TempData["Input.HullEngine"] as string);

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

                var ship = _shipBuilder.SetName("Temp")
                    .SetHull(model.Input.HullId)
                    .SetEngine(model.Input.EngineId)
                    .AddWing(model.Input.Wings.Select(wing => new KeyValuePair<int, int[]>(wing.WingId, wing.Weapons)))
                    .Build();

                var errors = ShipValidator.Validate(ship, license).ToList();

                if (!errors.Any())
                {
                    TempData["Input.HullEngineWings"] = JsonSerializer.Serialize(model.Input);
                    return RedirectToAction(nameof(Confirm));
                }

                foreach (var error in errors)
                {
                    ModelState.AddModelError("Ship", error);
                }
            }

            return View(BuildWingsViewModel(model.Input));
        }

        [HttpGet]
        public async Task<IActionResult> Confirm()
        {
            if (TempData.ContainsKey("Input.HullEngineWings"))
            {
                var hullEngineWingsInput = JsonSerializer.Deserialize<WingsViewModel.InputModel>(TempData["Input.HullEngineWings"] as string);

                var ship = _shipBuilder.SetName(await _userHelper.GetUserName(User))
                    .SetHull(hullEngineWingsInput.HullId)
                    .SetEngine(hullEngineWingsInput.EngineId)
                    .AddWing(hullEngineWingsInput.Wings.Select(wing => new KeyValuePair<int, int[]>(wing.WingId, wing.Weapons)))
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

        #region ApiHelpers

        #region HullEngine

        private HullEngineViewModel BuildHullEngineViewModel()
        {
            return new()
            {
                Hulls = _transitAuthority.GetHulls(),
                Engines = _transitAuthority.GetEngines()
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
                Weapons = _transitAuthority.GetWeapons()
            };
        }

        private WingsViewModel BuildWingsViewModel(HullEngineViewModel.InputModel inputModel)
        {
            var model = BuildWingsViewModel();

            model.Input = new WingsViewModel.InputModel
            {
                HullId = inputModel.HullId,
                EngineId = inputModel.EngineId,
                WingCount = inputModel.WingCount,
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