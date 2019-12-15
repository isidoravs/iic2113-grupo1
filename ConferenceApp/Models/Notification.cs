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
        
        public string SenderId { get; set; }  // Al final voy a guardar el sender full name aqui jeje

        public string ReceiverId { get; set; }
        
        public bool Seen { get; set; }
        
        [Required]
        public bool IsEventNotification { get; set; }

        public string EventName { get; set; }
        
        public int EventId { get; set; }
        
        public string ConferenceName { get; set; }
        
        public int ConferenceId { get; set; }

        public Notification(string subject, string message, string senderId, string receiverId)
        {
            Subject = subject;
            Message = message;
            ReceiverId = receiverId;
            SenderId = senderId;
            
            // eliminar esto
            IsEventNotification = false;
        }
    }
}