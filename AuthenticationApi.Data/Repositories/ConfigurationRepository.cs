using AuthenticationApi.Common.Contracts.Repositories;
using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Data.Repositories
{
    public class ConfigurationRepository : GenericRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}