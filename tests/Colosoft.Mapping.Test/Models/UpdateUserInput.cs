namespace Colosoft.Mapping.Test.Models
{
    internal class UpdateUserInput
    {
        public string? Name { get; set; }

        public IList<UpdateUserClaim> Claims { get; } = new List<UpdateUserClaim>();
    }
}
