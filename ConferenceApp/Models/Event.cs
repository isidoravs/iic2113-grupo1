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
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        public int ConferenceVersionId { get; set; }

        public ICollection<Role> Roles { get; set; }
        
        public ICollection<Feedback> Feedbacks { get; set; }
        
        public List<CheckBoxItem> AvailableTags { get; set; }
        
        public int FileId { get; set; }

        public void NotifyAttendees(string message)
        {
        }
        
        public void AddReview(User user, Feedback feedback)
        {
        }
        
        public IDictionary<string, float> GetStatistics()
        {
            var dictionary = new Dictionary<string, float>();
            return dictionary;
        }
        public int RoomId { get; set; }
    }
}