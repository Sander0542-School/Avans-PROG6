namespace ShipsInSpace.Web.Models.Ship
{
    public class ConfirmViewModel
    {
        public GalacticSpaceTransitAuthority.Ship Ship { get; set; }

        public WingsViewModel.InputModel Input { get; set; }
    }
}