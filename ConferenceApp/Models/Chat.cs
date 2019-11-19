using System.ComponentModel.DataAnnotations;

namespace ConferenceApp.Models
{
    public class Chat : Event
    {
        [Required]
        public string Topic { get; set; }
        
        public string Panelists { get; set; }
        
        public string Moderator { get; set; }
    }
}