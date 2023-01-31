namespace Accounts.Core.DTO.Requests
{
    public class RegisterRequest : UserRequest
    {
        public Guid? ProfileId { get; set;}
    }
}