using ShipsInSpace.Logic.Licenses;

namespace ShipsInSpace.Logic.Extensions
{
    public static class PilotLicensesExtensions
    {
        public static int GetMaxWeight(this PilotLicense license)
        {
            return license switch
            {
                PilotLicense.A => 1000,
                PilotLicense.B => 1500,
                PilotLicense.C => 2000,
                PilotLicense.Z => int.MaxValue
            };
        }
    }
}