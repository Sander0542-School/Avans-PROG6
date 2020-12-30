using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipsInSpace.Data.Models
{
    public class User : IdentityUser
    {
        public string SecretKey { get; set; }
        public PilotLicenses PilotLicense { get; set; }

        public enum PilotLicenses { 
            A,
            B,
            C,
            Z
        }
    }
}
