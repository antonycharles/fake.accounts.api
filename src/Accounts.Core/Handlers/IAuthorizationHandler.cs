using System.Threading.Tasks;
using Accounts.Core.DTO.Requests;
using Accounts.Core.DTO.Responses;

namespace Accounts.Core.Handlers
{
    public interface IUserAuthorizationHandler
    {
        Task<TokenResponse> RegisterAsync(RegisterRequest request);
        Task<TokenResponse> AuthenticationAsync(LoginRequest request);
    }
}