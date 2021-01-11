using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GalacticSpaceTransitAuthority;
using ShipsInSpace.Logic.Validators.Attributes;

namespace ShipsInSpace.Web.Models.Ship
{
    public class HullEngineViewModel
    {
        public IEnumerable<Hull> Hulls { get; set; }

        public IEnumerable<Engine> Engines { get; set; }

        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [HullExists]
            [Display(Name = "Hull")]
            public int HullId { get; set; }

            [Required]
            [EngineExists]
            [Display(Name = "Engine")]
            public int EngineId { get; set; }

            [Required]
            [Range(2, int.MaxValue)]
            [EvenCount]
            [Display(Name = "Wing Count")]
            public int WingCount { get; set; }
        }
    }
}