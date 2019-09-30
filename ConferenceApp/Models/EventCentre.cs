using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceApp.Models
{
  public class EventCentre
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Location { get; set; }
    public string MapImage { get; set; }
    [Required]
    public float Latitude { get; set; }
    [Required]
    public float Longitude { get; set; }
  }
}
