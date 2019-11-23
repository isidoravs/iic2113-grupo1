using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ConferenceApp.Models
{
    public class PracticalSession : Event
    {
        [Required]
        public string Topic { get; set; }

        public List<EventTag> EventTags { get; set; }
        public string Exhibitor { get; set; }
    }
}