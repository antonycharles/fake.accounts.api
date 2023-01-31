using Accounts.Core.DTO.Requests;
using Accounts.Core.DTO.Responses;

namespace Accounts.Core.Handlers
{
    public interface IUserHandler
    {
        Task<UserResponse> CreateAsync(UserRequest authorizationRequest);
        Task<UserResponse> GetOrCreateByEmailAsync(UserRequest authorizationRequest);
    }
}