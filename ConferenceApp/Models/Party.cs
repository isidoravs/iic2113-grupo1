using System.ComponentModel.DataAnnotations;

namespace ConferenceApp.Models
{
    public class Party : Event
    {
        [Required]
        public string MusicStyle { get; set; }
    }
}