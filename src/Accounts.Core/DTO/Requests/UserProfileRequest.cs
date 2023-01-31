namespace Accounts.Core.DTO.Requests
{
    public class UserProfileRequest
    {
        public Guid UserId { get; set; }
        public Guid? PrifileId { get; set; }
        public Guid AppId { get; set; }
    }
}