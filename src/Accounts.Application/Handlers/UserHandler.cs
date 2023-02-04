using Accounts.Application.Exceptions;
using Accounts.Application.Mappers.UserMappers;
using Accounts.Core.DTO.Requests;
using Accounts.Core.DTO.Responses;
using Accounts.Core.Enums;
using Accounts.Core.Handlers;
using Accounts.Core.Providers;
using Accounts.Core.Repositories;

namespace Accounts.Application.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordProvider _passwordProvider;

        public UserHandler(IUserRepository userRepository, IPasswordProvider passwordProvider = null)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordProvider = passwordProvider ?? throw new ArgumentNullException(nameof(passwordProvider));
        }

        public async Task<UserResponse> CreateAsync(UserRequest userRequest)
        {
            var userExist = await _userRepository.AnyAsync(w => w.Email == userRequest.Email);
             
            if(userExist)
                throw new NotFoundException("User already exists");

            var salt = _passwordProvider.GenerateSalt();
            string passwordHash = _passwordProvider.HashPassword(userRequest.Password, salt);

            var user = userRequest.ToUser();
            user.PasswordHash = passwordHash;
            user.Salt = salt;
            user.Status = StatusEnum.Active;
            user.CreatedAt = DateTime.UtcNow;

            user = await _userRepository.AddAsync(user);

            return user.ToResponse();
        }

        public async Task<UserResponse> GetOrCreateByEmailAsync(UserRequest request)
        {
            var user = await _userRepository.GetByEmail(request.Email);
            
            if(user != null)
                user.ToResponse();

            return await CreateAsync(request);
        }
    }
}