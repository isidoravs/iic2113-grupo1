using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceApp.Models
{
    public class Talk : Event
    {
        [Required]
        public string Topic { get; set; }
        
        [Required]
        public string ComplementaryMaterial { get; set; }
    }
}