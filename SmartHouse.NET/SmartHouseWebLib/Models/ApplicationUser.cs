using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseWebLib.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(255)]        
        [Required]
        public string Name { get; set; }

        [MaxLength(255)]
        [Required]
        public string Surname { get; set; }

        public int HouseId { get; set; }

        public House House { get; set; }

        public virtual ICollection<UserLocation> UserLocations { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

}
