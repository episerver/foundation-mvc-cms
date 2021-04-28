using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Foundation.Infrastructure.Cms.Users
{
    public class SiteUser : ApplicationUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string RegistrationSource { get; set; }

        [NotMapped] public string Password { get; set; }

        public bool NewsLetter { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SiteUser> manager)
        {
            //// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            //var userIdentity = await manager.(this);

            //// Add custom user claims here
            //userIdentity.AddClaim(new Claim(ClaimTypes.Email, Email));

            //if (!string.IsNullOrEmpty(FirstName))
            //{
            //    userIdentity.AddClaim(new Claim(ClaimTypes.GivenName, FirstName));
            //}

            //if (!string.IsNullOrEmpty(LastName))
            //{
            //    userIdentity.AddClaim(new Claim(ClaimTypes.Surname, LastName));
            //}

            return null;
        }
    }
}