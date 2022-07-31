using AuthenticationApi.Common.Contracts.Repositories;
using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Data.Repositories
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}