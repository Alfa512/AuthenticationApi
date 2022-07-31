using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationApi.Common.Contracts.Data;
using AuthenticationApi.Common.Contracts.Services;
using AuthenticationApi.Model.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Business.Services
{
    public class DatabaseConfigurationService : ServiceBase, IDatabaseConfigurationService
    {
        private readonly IDataContext _context;

        private const string PasswordSecret = "PasswordSecret";

        public DatabaseConfigurationService(IDataContext context)
        {
            _context = context;
        }

        public async Task<List<Configuration>> GetConfiguration()
        {
            var result = await _context.Configuration.GetAll().ToListAsync();
            
            return result;
        }

        public async Task<Configuration> GetConfigByName(string name)
        {
            return await _context.Configuration.GetAll().FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<string> GetPasswordSecret()
        {
            return await _context.Configuration.GetAll().Where(p => p.Name == PasswordSecret).Select(r => r.Value).FirstOrDefaultAsync();
        }


    }
}