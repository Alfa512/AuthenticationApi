using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationApi.Model.Data
{
    [Table("Users")]
    public class User : IdentityUser<long>
    {
        public User()
        {
            UserIdentifier = Guid.NewGuid();
            Gender = true;
            RegistrationDate = DateTime.Now;
            FirstName = "";
            LastName = "";
            RegistrationComplete = false;
        }

        public void SetDefaultValues()
        {
            UserIdentifier = Guid.NewGuid();
            Gender = true;
            RegistrationDate = DateTime.Now;
            EmailConfirmed = false;
            PhoneNumberConfirmed = false;
            TwoFactorEnabled = false;
            LockoutEnabled = false;
            AccessFailedCount = 0;
            RegistrationComplete = false;
        }
        //public long Id { get; set; }
        public Guid UserIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RestorePasswordToken { get; set; }
        public DateTime? TokenValidTo { get; set; }
        public bool RegistrationComplete { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }
        //public string Email { get; set; }
        //public string PhoneNumber { get; set; }

        //public virtual ICollection<IdentityRole<long>> Roles { get; set; } TODO: Add Role Entity
        //public virtual ICollection<IdentityUserClaim<long>> Claims { get; set; }
        public virtual ICollection<Login> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<long>> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public string GetUserName()
        {
            return UserName;
        }
        public string GetPasswordHash()
        {
            return PasswordHash;
        }
    }
}