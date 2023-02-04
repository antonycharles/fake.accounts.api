using Accounts.Core.DTO.Requests;
using Accounts.Core.Handlers;
using Accounts.Core.Providers;
using Accounts.Core.Repositories;

namespace Accounts.Application.Handlers
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHandler _userHandler;
        private readonly IUserProfileHandler _userProfileHandler;
        private readonly IPasswordProvider _passwordProvider;

        public AuthorizationHandler(
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
    }
}