
using System.Security.Claims;
using System.Security.Cryptography;
using Accounts.Core.DTO.Responses;
using Accounts.Core.Handlers;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.Application.Handlers
{
    public class TokenHandler : ITokenHandler
    {
        private readonly ITokenKeyHandler _tokenKeyHandler;

        public TokenHandler(ITokenKeyHandler tokenKeyHandler)
        {
            _tokenKeyHandler = tokenKeyHandler;
        }

        public TokenResponse Create()
        {
            var tokenHandler = new JsonWebTokenHandler();

            var key = _tokenKeyHandler.GetKey();

            var jwt = new SecurityTokenDescriptor
            {
                Issuer = "www.mysite.com",
                Audience = "your-spa",
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddHours(1),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, "meuemail@gmail.com", ClaimValueTypes.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, "Bruno Brito"),
                    new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                    new Claim("roles", "teste 1"),
                    new Claim("roles", "teste 2"),
                    new Claim("roles", "teste 3"),
                    new Claim("roles", "teste 4")
                })
            };

            jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.EcdsaSha256);
            var lastJws = tokenHandler.CreateToken(jwt);

            return new TokenResponse
            {
                ExpiresIn = jwt.Expires,
                Token = lastJws
            };
        }
    }
}