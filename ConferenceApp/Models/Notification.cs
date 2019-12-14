using System;
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
        
        public string Subject { get; set; }

        [Required] 
        public string Message { get; set; }

        [Required]
        public string ReceiverId { get; set; }
        
        public bool Seen { get; set; }
        
        [Required]
        public bool IsEventNotification { get; set; }

        public Event Event { get; set; }
        
        public Conference Conference { get; set; }
    }
}