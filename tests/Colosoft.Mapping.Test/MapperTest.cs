using Colosoft.Mapping.Test.Models;

namespace Colosoft.Mapping.Test
{
    public class MapperTest
    {
        [Fact]
        public async Task MapTest()
        {
            var claimMapper = new UpdateUserClaimMapper();
            var updateUserInputMapper = new UpdateUserInputMapper(claimMapper);

            var user = new User
            {
                Name = "user1",
            };

            user.Claims.Add(new Claim("type1", "123"));
            user.Claims.Add(new Claim("type2", "456"));

            var input = new UpdateUserInput
            {
                Name = "user123",
            };

            input.Claims.Add(new UpdateUserClaim("type2", "444"));
            input.Claims.Add(new UpdateUserClaim("type3", "789"));

            await updateUserInputMapper.Apply(input, user, default);

            Assert.Equal(2, user.Claims.Count);
            Assert.DoesNotContain(user.Claims, f => f.ClaimType == "Type1");
        }
    }
}
