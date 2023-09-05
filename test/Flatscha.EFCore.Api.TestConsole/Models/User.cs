namespace Flatscha.EFCore.Api.TestConsole.Models
{
    public partial class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Title { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime? LastLogin { get; set; }

        public bool ObjectIsDeleted { get; set; }

        public DateTime ObjectCreatedDateTime { get; set; }

        public Guid ObjectCreatedUserId { get; set; }

        public DateTime ObjectChangedDateTime { get; set; }

        public Guid ObjectChangedUserId { get; set; }
    }
}
