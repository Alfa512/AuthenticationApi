using System;
using System.Collections.Generic;
using AuthenticationApi.Common.Contracts.Data;
using AuthenticationApi.Common.Contracts.Repositories;
using AuthenticationApi.Common.Contracts.Services;
using AuthenticationApi.Data.Repositories;
using AuthenticationApi.Model.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Data
{
    public class ApplicationDbContext : DbContext, IDataContext
    {
        private IConfigurationService _configurationService;
        public ApplicationDbContext(IConfigurationService configurationService) //: base("MainConnection")
        {
            _configurationService = configurationService;
            //Database.EnsureCreated();
            //Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            //Users = new UserRepository(this);
            //Configuration = new ConfigurationRepository(this);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configurationService.MainConnection);
            }
        }

        void IDataContext.Commit()
        {
            SaveChanges();
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }
        //public DbSet<PostPreview> PostPreviews { get; set; }

        //public virtual DbSet<User> Users { get; set; }
        IUserRepository IDataContext.Users => new UserRepository(this);
        IUserRoleRepository IDataContext.UserRoles => new UserRoleRepository(this);
        //IRoleRepository IDataContext.Roles => new RoleRepository(this); TODO: Add Repo for Roles
        IConfigurationRepository IDataContext.Configuration => new ConfigurationRepository(this);

        //public virtual DbSet<IdentityRole<long>> Roles { get; set; }
        //public virtual DbSet<IdentityUserClaim<long>> Claims { get; set; }
        //public virtual DbSet<Login> Logins { get; set; }
        //public virtual DbSet<LoginProvider> LoginProviders { get; set; }
        public virtual DbSet<IdentityUserToken<long>> Tokens { get; set; }
        //public virtual DbSet<IdentityUserRole<long>> UserRoles { get; set; }

        //IConfigurationRepository IDataContext.Configuration => new ConfigurationRepository(this);
        //IUserRepository IDataContext.Users => new UserRepository(this);
        IImageRepository IDataContext.Images => new ImageRepository(this);
        ILoginRepository IDataContext.Logins => new LoginRepository(this);
        ILoginProviderRepository IDataContext.LoginProviders => new LoginProviderRepository(this);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder..Conventions.Remove<PluralizingTableNameConvention>();
            //builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //builder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            #region OneToOneRelations


            #endregion

            #region OneToManyRelations

            #endregion

            #region ManyToOneRelations

            //builder.Entity<User>().HasMany(p => p.Posts).WithOne(p => p.Author)
            //    .HasPrincipalKey(u => u.Id).HasForeignKey(p => p.CreatedBy);

            //builder.Entity<User>().HasMany(p => p.Posts).WithOne(p => p.Author)
            //    .HasPrincipalKey(u => u.Id).HasForeignKey(p => p.CreatedBy);

            #endregion

            builder.Entity<User>(b =>
            {
                // Each User can have many UserClaims
                //b.HasMany(e => e.Claims)
                //    .WithOne()
                //    .HasForeignKey(uc => uc.UserId)
                //    .HasPrincipalKey(u => u.Id)
                //    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .HasPrincipalKey(u => u.Id)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .HasPrincipalKey(u => u.Id)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .HasPrincipalKey(u => u.Id)
                    .IsRequired();

                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .HasPrincipalKey(u => u.Id)
                    .IsRequired();
            });

            //builder.Entity<IdentityRole<long>>(b =>
            //{
            //    b.HasKey(r => r.Id);
            //    //b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
            //    b.ToTable("Roles");
            //    b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            //    b.Property(u => u.Name).HasMaxLength(256);
            //    b.Property(u => u.NormalizedName).HasMaxLength(256);

            //    b.HasMany<IdentityUserRole<long>>().WithOne().HasForeignKey(ur => ur.RoleId).HasPrincipalKey(p => p.Id).IsRequired();
            //});

            //builder.Entity<IdentityUserClaim<long>>(b =>
            //{
            //    b.HasKey(rc => rc.Id);
            //    b.ToTable("Claims");
            //});

            //builder.Entity<IdentityUserRole<long>>(b =>
            //{
            //    b.HasKey(r => new { r.UserId, r.RoleId });
            //    b.ToTable("UserRoles");
            //});

            //builder.Entity<IdentityUserLogin<long>>(b =>
            //{
            //    b.HasKey(r => new { r.LoginProvider, r.ProviderKey, r.UserId });
            //    b.ToTable("Logins");
            //});

            builder.Entity<IdentityUserToken<long>>(b =>
            {
                b.HasKey(r => new { r.UserId, r.LoginProvider });
                b.ToTable("Tokens");
            });

            builder.Entity<User>()
                .HasKey(e => new { e.Id });



            builder.Entity<Configuration>(b =>
            {
                b.HasKey(r => new { r.Id });
                b.ToTable("Configuration");
            });


            builder.Entity<Login>(b =>
            {
                b.HasKey(r => new { r.Id });
                b.ToTable("Logins");
            });


            builder.Entity<LoginProvider>(b =>
            {
                b.HasKey(r => new { r.Id });
                b.ToTable("LoginProviders");

                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(m => m.LoginProviderId)
                    .HasPrincipalKey(o => o.Id)
                    .IsRequired();
            });

            builder.Entity<UserRole>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId });
                b.ToTable("UserRoles");
            });



            base.OnModelCreating(builder);
        }
    }
}