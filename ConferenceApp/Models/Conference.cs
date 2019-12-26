using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceApp.Models
{
    public class Conference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        
        public string OrganizerId { get; set; }
        
        public ICollection<Sponsor> Sponsors { get; set; }
        
        public ICollection<ConferenceVersion> Versions { get; set; }
    }
}