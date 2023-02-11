using Accounts.Application.Exceptions;
using Accounts.Core.DTO.Requests;
using Accounts.Core.DTO.Responses;
using Accounts.Core.Handlers;
using Accounts.Core.Providers;
using Accounts.Core.Repositories;

namespace Accounts.Application.Handlers
{
    public class UserAuthorizationHandler : IUserAuthorizationHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHandler _userHandler;
        private readonly IUserProfileHandler _userProfileHandler;
        private readonly IPasswordProvider _passwordProvider;
        private readonly ITokenHandler _tokenHandler;

        public UserAuthorizationHandler(
            IUserRepository userRepository,
            IUserHandler userHandler,
            IUserProfileHandler userProfileHandler,
            IPasswordProvider passwordProvider,
            ITokenHandler tokenHandler)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userHandler = userHandler ?? throw new ArgumentNullException(nameof(userHandler));
            _userProfileHandler = userProfileHandler ?? throw new ArgumentNullException(nameof(userProfileHandler));
            _passwordProvider = passwordProvider ?? throw new ArgumentNullException(nameof(passwordProvider));
            _tokenHandler = tokenHandler;
        }

        public async Task<TokenResponse> RegisterAsync(RegisterRequest request)
        {
            var user = await _userHandler.GetOrCreateByEmailAsync(request);

            _ = await _userProfileHandler.CreateAsync(new UserProfileRequest{
                UserId = user.Id,
                PrifileId = request.ProfileId,
                AppId = request.AppId
            });

            return await AuthenticationAsync(new LoginRequest{
                Email = request.Email,
                Password = request.Password,
                AppId = request.AppId
            });
        }

        public async Task<TokenResponse> AuthenticationAsync(LoginRequest request)
        {
            var userDb = await _userRepository.GetByEmail(request.Email);

            var passwordHash = _passwordProvider.HashPassword(request.Password, userDb.Salt);

            if(userDb == null || userDb.PasswordHash != passwordHash)
                throw new ConflictException("User or password invalid");
                
            return _tokenHandler.Create();
        }
    }
}