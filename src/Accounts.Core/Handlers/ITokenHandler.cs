
using Accounts.Core.DTO.Responses;

namespace Accounts.Core.Handlers
{
    public interface ITokenHandler
    {
        TokenResponse Create();
    }
}