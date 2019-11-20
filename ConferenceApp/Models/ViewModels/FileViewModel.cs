using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;    
using ConferenceApp.Models;

namespace ConferenceApp.Models.ViewModels
{
    public class FileViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [NotMapped]
        public IFormFile MyFile { set; get; }
        
        public string FileDescription { set;get; }
        
        [Required]
        public  int EventId { set; get; }
    }
}