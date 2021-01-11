using System.ComponentModel.DataAnnotations;

namespace ShipsInSpace.Web.Models.Pirates
{
    public class LetterViewModel : PirateModel
    {
        [Display(Name = "Secret Key")]
        public string SecretKey { get; set; }
    }
}