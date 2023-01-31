using Accounts.Core.DTO.Requests;

namespace Accounts.Core.Handlers
{
    public interface IAuthorizationHandler
    {
        Task RegisterAsync(RegisterRequest request);
        Task ValidateAsync(LoginRequest request);
    }
}