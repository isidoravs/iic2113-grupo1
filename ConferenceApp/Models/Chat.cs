using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ConferenceApp.Models
{
    public class Chat : Event
    {
        [Required]
        public string Topic { get; set; }
        
        public List<EventTag> EventTags { get; set; }
        public string Panelists { get; set; }
        
        public string Moderator { get; set; }
    }
}