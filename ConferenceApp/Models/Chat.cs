using System.ComponentModel.DataAnnotations;

namespace ConferenceApp.Models
{
    public class Chat : Event
    {
        [Required]
        public string Topic { get; set; }
    }
}