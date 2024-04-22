using Colosoft.Mapping.Test.Models;

namespace Colosoft.Mapping.Test
{
    public class MergeCollectionExtensionsTest
    {
        [Fact]
        public async Task GivenClaimsToMergeAsync()
        {
            var claims1 = new List<Claim>
            {
                new ("Type1", "123"),
                new ("Type2", "456"),
            };

            var claims2 = new List<Claim>
            {
                new ("Type2", "444"),
                new ("Type3", "789"),
            };

            await claims1.MergeToAsync(
                claims2,
                f => f.ClaimType,
                f => f.ClaimType,
                (f, _) =>
                {
                    return Task.FromResult(new Claim(f.ClaimType, f.ClaimValue));
                },
                (x, y, _) =>
                {
                    y.ClaimValue = x.ClaimValue;
                    return Task.CompletedTask;
                },
                default);

            Assert.Equal(2, claims1.Count);
            Assert.DoesNotContain(claims1, f => f.ClaimType == "Type1");
        }
    }
}