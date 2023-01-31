using System;
using Accounts.Core.DTO.Requests;

namespace Accounts.Application.Mappers.UserProfileMappers
{
    public static class UserProfileRequestMapper
    {
        public static Core.Entities.UserProfile ToUserProfile(this UserProfileRequest request) => new Core.Entities.UserProfile
        {
            ProfileId = request.PrifileId != null ? request.PrifileId.Value : Guid.Empty,
            UserId = request.UserId
        };
    }
}