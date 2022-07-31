using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationApi.Model.Data;

namespace AuthenticationApi.Common.Contracts.Services
{
    public interface IDatabaseConfigurationService
    {
        Task<List<Configuration>> GetConfiguration();
        Task<Configuration> GetConfigByName(string name);
        Task<string> GetPasswordSecret();
    }
}