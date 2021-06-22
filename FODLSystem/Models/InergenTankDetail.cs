using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class InergenTankDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int ItemId { get; set; }
        public virtual Item Items { get; set; }
        public int Cylinder { get; set; }
        public int Gauge { get; set; }
        public int Hose { get; set; }
        public int Pressure { get; set; }

        public string Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int InergenTankHeaderId { get; set; }
        public virtual InergenTankHeader InergenTankHeaders { get; set; }
        public string InspectedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string NotedBy { get; set; }
        public string ImageUrl { get; set; }

        public int LocationInergenTankId { get; set; }
        public virtual LocationInergenTank Locations { get; set; }


    }
}
