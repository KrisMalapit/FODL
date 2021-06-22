using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class EmergencyLightDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    
        public int ItemId { get; set; }
        public virtual Item Items { get; set; }
        public int Battery { get; set; }
        public int Bulb { get; set; }
        public int Usable { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int EmergencyLightHeaderId { get; set; }
        public virtual EmergencyLightHeader EmergencyLightHeaders { get; set; }
        public string InspectedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string NotedBy { get; set; }
        public string ImageUrl { get; set; }
        public int LocationEmergencyLightId { get; set; }
        public virtual LocationEmergencyLight Locations { get; set; }

    }
}
