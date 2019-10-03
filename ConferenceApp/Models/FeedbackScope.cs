using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceApp.Models
{
    public class FeedbackScope
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Grade { get; set; }
        
        public ICollection<FeedbackCategory> FeedbackCategories { get; set; }
    }
}