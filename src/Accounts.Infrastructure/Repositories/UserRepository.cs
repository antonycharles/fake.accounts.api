using Accounts.Core.Entities;
using Accounts.Core.Repositories;
using Accounts.Infrastructure.Data;
using Accounts.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AccountsContext dbContext) : base(dbContext)
        {

        }

        public async Task<User> GetByEmail(string email)
        {
            return await _table.FirstOrDefaultAsync(w => w.Email == email);
        }
    }
}