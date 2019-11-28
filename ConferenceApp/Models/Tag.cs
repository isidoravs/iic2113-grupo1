using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ConferenceApp.Models
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [RegularExpression(@"^[a-zA-Z0-9]{2,20}$", ErrorMessage = "Name can contain only letters and numbers, and 1 < length < 21")]
        [Required]
        public string Name { get; set; }
        
        public List<EventTag> EventTags { get; set; }
    }
}