using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ConferenceApp.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] 
        public string Message { get; set; }
        
        [Required]
        public string SenderId { get; set; }
        
        [Required]
        public string ReceiverId { get; set; }
    }
}