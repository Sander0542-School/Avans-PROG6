using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ShipsInSpace.Data.Models.User;

namespace ShipsInSpace.Web.Models.Users
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; }

        [Required]
        [Display(Name = "Pilot License")]
        public PilotLicenses PilotLicense { get; set; }
    }
}
