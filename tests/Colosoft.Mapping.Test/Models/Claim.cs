namespace Colosoft.Mapping.Test.Models
{
    internal class Claim
    {
        public Claim(string claimType, string claimValue)
        {
            this.ClaimType = claimType;
            this.ClaimValue = claimValue;
        }

        public Claim()
        {
            this.ClaimType = string.Empty;
            this.ClaimValue = string.Empty;
        }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public override string ToString()
        {
            return $"{this.ClaimType}:{this.ClaimValue}";
        }
    }
}