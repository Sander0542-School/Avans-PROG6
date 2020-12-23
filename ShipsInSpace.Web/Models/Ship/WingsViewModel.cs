using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GalacticSpaceTransitAuthority;
using ShipsInSpace.Logic.Validators.Attributes;

namespace ShipsInSpace.Web.Models.Ship
{
    public class WingsViewModel
    {
        public IEnumerable<Wing> Wings { get; set; }

        public IEnumerable<Weapon> Weapons { get; set; }

        public InputModel Input { get; set; }

        public class InputModel : HullEngineViewModel.InputModel
        {
            [Required]
            [EvenCount]
            [Display(Name = "Wings")]
            public InputWing[] Wings { get; set; }
        }

        public class InputWing
        {
            [Required]
            [WingExists]
            [Display(Name = "Wing")]
            public int WingId { get; set; }

            [Required]
            [WeaponsExist]
            [Display(Name = "Weapons")]
            public int[] Weapons { get; set; }
        }
    }
}