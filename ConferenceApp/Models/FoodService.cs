using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConferenceApp.Models
{
    public class FoodService : Event
    {
        [Required]
        public string Category { get; set; }
        
        public ICollection<MenuOption> MenuOptions { get; set; }
    }
}