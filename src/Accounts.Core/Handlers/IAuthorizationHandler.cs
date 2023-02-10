using System.Threading.Tasks;
using Accounts.Core.DTO.Requests;

namespace Accounts.Core.Handlers
{
    public interface IUserAuthorizationHandler
    {
        Task RegisterAsync(RegisterRequest request);
        Task ValidateAsync(LoginRequest request);
    }
}