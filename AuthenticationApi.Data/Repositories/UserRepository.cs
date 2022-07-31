using AuthenticationApi.Common.Contracts.Repositories;
using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}