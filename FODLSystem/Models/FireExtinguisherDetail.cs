using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class FireExtinguisherDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public virtual Item Items { get; set; }
        public int Cylinder { get; set; }
        public int Lever { get; set; }
        public int Gauge { get; set; }
        public int SafetySeal { get; set; }
        public int Hose { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int FireExtinguisherHeaderId { get; set; }
        public virtual FireExtinguisherHeader FireExtinguisherHeaders { get; set; }
        public string InspectedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string NotedBy { get; set; }
        public string ImageUrl { get; set; }
        public int LocationFireExtinguisherId { get; set; }
        public virtual LocationFireExtinguisher Locations { get; set; }

    }
}
