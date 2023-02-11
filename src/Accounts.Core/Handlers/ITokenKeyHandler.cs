
using Microsoft.IdentityModel.Tokens;

namespace Accounts.Core.Handlers
{
    public interface ITokenKeyHandler
    {
        ECDsaSecurityKey GetKey();
        JsonWebKey GetPublicKey();
    }
}