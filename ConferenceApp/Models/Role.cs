using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceApp.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        
        public Role()
        { 
            Name = "attendant";
        }
    }
}