using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GalacticSpaceTransitAuthority;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShipsInSpace.Logic;
using ShipsInSpace.Logic.Validators;
using ShipsInSpace.Web.Models.Ship;

namespace ShipsInSpace.Web.Controllers
{
    [Authorize]
    public class ShipController : Controller
    {
        private ISpaceTransitAuthority _transitAuthority;
        private ShipBuilder _shipBuilder;

        public ShipController(ISpaceTransitAuthority transitAuthority, ShipBuilder shipBuilder)
        {
            _transitAuthority = transitAuthority;
            _shipBuilder = shipBuilder;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(HullEngine));
        }

        [HttpGet]
        public IActionResult HullEngine()
        {
            return View(new HullEngineViewModel
            {
                Hulls = _transitAuthority.GetHulls(),
                Engines = _transitAuthority.GetEngines()
            });
        }

        [HttpPost]
        public IActionResult HullEngine(HullEngineViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["Input.HullEngine"] = JsonSerializer.Serialize(model.Input);
                return RedirectToAction(nameof(Wings));
            }

            model.Hulls = _transitAuthority.GetHulls();
            model.Engines = _transitAuthority.GetEngines();

            return View(model);
        }

        [HttpGet]
        public IActionResult Wings()
        {
            if (TempData.ContainsKey("Input.HullEngine"))
            {
                var hullEngineInput = JsonSerializer.Deserialize<HullEngineViewModel.InputModel>(TempData["Input.HullEngine"] as string);

                return View(new WingsViewModel
                {
                    Wings = _transitAuthority.GetWings(),
                    Weapons = _transitAuthority.GetWeapons(),

                    Input = new WingsViewModel.InputModel
                    {
                        HullId = hullEngineInput.HullId,
                        EngineId = hullEngineInput.EngineId,
                    }
                });
            }

            return RedirectToAction(nameof(HullEngine));
        }

        [HttpPost]
        public IActionResult Wings(WingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ship = _shipBuilder.SetName("Temp")
                    .SetHull(model.Input.HullId)
                    .SetEngine(model.Input.EngineId)
                    .AddWing(model.Input.Wings.Select(wing => new KeyValuePair<int, int[]>(wing.WingId, wing.Weapons)))
                    .Build();

                var errors = ShipValidator.Validate(ship).ToList();

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

            model.Wings = _transitAuthority.GetWings();
            model.Weapons = _transitAuthority.GetWeapons();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Confirm()
        {
            if (TempData.ContainsKey("Input.HullEngineWings"))
            {
                var hullEngineWingsInput = JsonSerializer.Deserialize<WingsViewModel.InputModel>(TempData["Input.HullEngineWings"] as string);

                var ship = _shipBuilder.SetName("Temp")
                    .SetHull(hullEngineWingsInput.HullId)
                    .SetEngine(hullEngineWingsInput.EngineId)
                    .AddWing(hullEngineWingsInput.Wings.Select(wing => new KeyValuePair<int, int[]>(wing.WingId, wing.Weapons)))
                    .Build();

                return Json(ship);
            }

            return RedirectToAction(nameof(HullEngine));
        }
    }
}