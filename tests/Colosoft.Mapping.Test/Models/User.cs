namespace Colosoft.Mapping.Test.Models
{
    internal class User
    {
        public string? Name { get; set; }

        public IList<Claim> Claims { get; } = new List<Claim>();
    }
}
