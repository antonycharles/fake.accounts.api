using System.Security.Claims;
using System.Security.Cryptography;
using Accounts.Core.DTO.Requests;
using Accounts.Core.Handlers;
using Accounts.Core.Providers;
using Accounts.Core.Repositories;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.Application.Handlers
{
    public class UserAuthorizationHandler : IUserAuthorizationHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHandler _userHandler;
        private readonly IUserProfileHandler _userProfileHandler;
        private readonly IPasswordProvider _passwordProvider;

        public UserAuthorizationHandler(
            IUserRepository userRepository,
            IUserHandler userHandler,
            IUserProfileHandler userProfileHandler,
            IPasswordProvider passwordProvider)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userHandler = userHandler ?? throw new ArgumentNullException(nameof(userHandler));
            _userProfileHandler = userProfileHandler ?? throw new ArgumentNullException(nameof(userProfileHandler));
            _passwordProvider = passwordProvider ?? throw new ArgumentNullException(nameof(passwordProvider));
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var user = await _userHandler.GetOrCreateByEmailAsync(request);

            _ = await _userProfileHandler.CreateAsync(new UserProfileRequest{
                UserId = user.Id,
                PrifileId = request.ProfileId,
                AppId = request.AppId
            });
        }

        public async Task ValidateAsync(LoginRequest request)
        {
            var userDb = await _userRepository.GetByEmail(request.Email);

            if(userDb == null || userDb.PasswordHash != _passwordProvider.HashPassword(request.Password, userDb.Salt))
                throw new Exception("User or password invalid");
        }

        public static void TEste()
        {
            var tokenHandler = new JsonWebTokenHandler();

            var key = new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP256))
            {
                KeyId = Guid.NewGuid().ToString(),
            };

            var Jwt = new SecurityTokenDescriptor
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


            Jwt.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.EcdsaSha256);
            var lastJws = tokenHandler.CreateToken(Jwt);
            Console.WriteLine($"{lastJws}{Environment.NewLine}");


            // Store in filesystem
            // Store HMAC os Filesystem, recover and test if it's valid
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

            File.WriteAllText("current-ecdsa.key", JsonConvert.SerializeObject(jwk));

            var storedJwk = JsonConvert.DeserializeObject<JsonWebKey>(File.ReadAllText("current-ecdsa.key"));
            TokenValidationParams.IssuerSigningKey = storedJwk;
            var validationResult = tokenHandler.ValidateToken(lastJws, TokenValidationParams);

            Console.WriteLine(validationResult.IsValid);
        }
    }
}