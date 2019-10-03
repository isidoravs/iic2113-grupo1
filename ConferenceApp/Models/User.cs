using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ConferenceApp.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public bool IsAdmin { get; set; }
        
        [Required]
        public string Age { get; set; }
        
        public string Cv { get; set; }
        
        public string Biography { get; set; }
        
        public string ContactInfo { get; set; }
        
        public ICollection<Notification> Notifications { get; set; }
        
    }
}