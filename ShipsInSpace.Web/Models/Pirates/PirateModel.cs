using System.ComponentModel.DataAnnotations;
using ShipsInSpace.Logic.Licenses;

namespace ShipsInSpace.Web.Models.Pirates
{
    public class PirateModel
    {
        public string Id { get; set; }

        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; }

        [Display(Name = "Pilot License")]
        public PilotLicense PilotLicense { get; set; }
    }
}