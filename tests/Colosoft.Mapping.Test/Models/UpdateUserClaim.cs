namespace Colosoft.Mapping.Test.Models
{
    internal class UpdateUserClaim
    {
        public UpdateUserClaim(string claimType, string claimValue)
        {
            this.ClaimType = claimType;
            this.ClaimValue = claimValue;
        }

        public UpdateUserClaim()
        {
            this.ClaimType = string.Empty;
            this.ClaimValue = string.Empty;
        }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}