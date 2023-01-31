namespace Accounts.Core.Entities
{
    public class UserProfile
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}