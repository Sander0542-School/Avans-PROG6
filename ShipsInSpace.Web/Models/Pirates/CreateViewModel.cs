using System.ComponentModel.DataAnnotations;
using ShipsInSpace.Logic.Licenses;

namespace ShipsInSpace.Web.Models.Pirates
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; }

        [Required]
        [Display(Name = "Pilot License")]
        public PilotLicense PilotLicense { get; set; }
    }
}
