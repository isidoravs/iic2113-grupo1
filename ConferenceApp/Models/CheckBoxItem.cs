using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceApp.Models
{
    public class CheckBoxItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int TagId { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public bool IsChecked { get; set; }
    }
}