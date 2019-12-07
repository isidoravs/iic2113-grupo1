using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ConferenceApp.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public bool IsAdmin { get; set; }
        
        public string Age { get; set; }
        
        public string Cv { get; set; }
        
        public string Biography { get; set; }
        
        public string ContactInfo { get; set; }
        
        public ICollection<Notification> Notifications { get; set; }
        
        public ICollection<Role> Roles { get; set; }
        
    }
}