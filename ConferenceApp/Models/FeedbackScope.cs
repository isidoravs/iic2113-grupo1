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
        [Range(1,5)]
        public int Grade { get; set; }
        
        public int FeedbackId { get; set; }
        public int FeedbackCategoryId { get; set; }
    }
}