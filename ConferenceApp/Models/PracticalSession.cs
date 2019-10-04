using System.ComponentModel.DataAnnotations;

namespace ConferenceApp.Models
{
    public class PracticalSession : Event
    {
        [Required]
        public string Topic { get; set; }
        
        [Required]
        public string ComplementaryMaterial { get; set; }
    }
}