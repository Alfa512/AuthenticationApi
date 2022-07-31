using System;
using AuthenticationApi.Common.Contracts.Repositories;
using AuthenticationApi.Model.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Common.Contracts.Data
{
    public interface IDataContext : IDisposable
    {
        void Commit();
        //DbSet<User> Users { get; }
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IConfigurationRepository Configuration { get; }
        IImageRepository Images { get; }
        ILoginRepository Logins { get; }
        ILoginProviderRepository LoginProviders { get; }
    }
}