using Colosoft.Mapping.Test.Models;

namespace Colosoft.Mapping.Test
{
    internal class UpdateUserInputMapper : Mapper<UpdateUserInput, User>
    {
        public UpdateUserInputMapper(IMapper<UpdateUserClaim, Claim> claimMapper)
        {
            this
                .Map(f => f.Name, f => f.Name)
                .Map(
                    f => f.Claims,
                    f => f.ClaimType,
                    f => f.Claims,
                    f => f.ClaimType,
                    f => new Claim(),
                    claimMapper.Apply);
        }
    }
}
