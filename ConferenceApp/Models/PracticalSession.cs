using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ConferenceApp.Models
{
    public class PracticalSession : Event
    {
        [Required]
        public string Topic { get; set; }
        
        [Required]
        public string ComplementaryMaterial { get; set; }
        
        public List<EventTag> EventTags { get; set; }
    }
}