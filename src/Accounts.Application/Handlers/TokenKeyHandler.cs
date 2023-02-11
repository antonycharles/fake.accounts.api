using System.Security.Cryptography;
using System.Text.Json;
using Accounts.Core.Handlers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.Application.Handlers
{
    public class TokenKeyHandler : ITokenKeyHandler
    {
        private readonly IDistributedCache _cache;
        private readonly ECDsa _key;
        private readonly string _keyId;

        public TokenKeyHandler(IDistributedCache cache)
        {
            _cache = cache;
            _key = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            _keyId = Guid.NewGuid().ToString();
        }

        public ECDsaSecurityKey GetKey()
        {
            return new ECDsaSecurityKey(_key)
            {
                KeyId = _keyId,
            };
        }

        public JsonWebKey GetPublicKey()
        {
            var key = GetKey();

            var parameters = key.ECDsa.ExportParameters(true);
            var jwk = new JsonWebKey()
            {
                Kty = JsonWebAlgorithmsKeyTypes.EllipticCurve,
                Use = "sig",
                Kid = key.KeyId,
                KeyId = key.KeyId,
                X = Base64UrlEncoder.Encode(parameters.Q.X),
                Y = Base64UrlEncoder.Encode(parameters.Q.Y),
                D = Base64UrlEncoder.Encode(parameters.D),
                Crv = JsonWebKeyECTypes.P256,
                Alg = "ES256"
            };

            return jwk;
        }
    }
}