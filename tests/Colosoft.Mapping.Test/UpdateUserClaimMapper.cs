using Colosoft.Mapping.Test.Models;

namespace Colosoft.Mapping.Test
{
    internal class UpdateUserClaimMapper : Mapper<UpdateUserClaim, Claim>
    {
        public UpdateUserClaimMapper()
        {
            this
                .Map(f => f.ClaimType, f => f.ClaimType)
                .Map(f => f.ClaimValue, f => f.ClaimValue);
        }
    }
}
