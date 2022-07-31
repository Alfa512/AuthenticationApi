using AuthenticationApi.Common.Contracts.Repositories;
using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Data.Repositories
{
    public class LoginProviderRepository : GenericRepository<LoginProvider>, ILoginProviderRepository
    {
        public LoginProviderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}