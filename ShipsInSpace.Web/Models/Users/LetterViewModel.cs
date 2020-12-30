using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShipsInSpace.Web.Models.Users
{
    public class LetterViewModel : ViewModel
    {
        [Display(Name = "Secret Key")]
        public string SecretKey { get; set; }
    }
}
