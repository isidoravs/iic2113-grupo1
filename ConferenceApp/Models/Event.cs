using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceApp.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public DateTime startDate { get; set; }
        
        [Required]
        public DateTime endDate { get; set; }

        public int ConferenceVersionId { get; set; }
        
        public void notifyAttendees(string Message)
        {
        }
        
        public void addReview(User user, Feedback feedback)
        {
        }
        
        public IDictionary<string, float> getStatistics()
        {
            var dictionary = new Dictionary<string, float>();
            return dictionary;
        }
    }
}