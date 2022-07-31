using AuthenticationApi.Common.Contracts.Repositories;
using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Data.Repositories
{
    public class LoginRepository : GenericRepository<Login>, ILoginRepository
    {
        public LoginRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}