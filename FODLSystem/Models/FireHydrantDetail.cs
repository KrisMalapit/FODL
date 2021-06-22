using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class FireHydrantDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //public int LocationFireHydrantId { get; set; }
        public int ItemId { get; set; }
        public virtual Item Items { get; set; }
        public int GlassCabinet { get; set; }
        public int Hanger { get; set; }
        public int Hose15 { get; set; }
        public int Nozzle15 { get; set; }
        public int Hose25 { get; set; }
        public int Nozzle25 { get; set; }
        public int SpecialTools { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int FireHydrantHeaderId { get; set; }
        public virtual FireHydrantHeader FireHydrantHeaders { get; set; }
        public string InspectedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string NotedBy { get; set; }
        public string ImageUrl { get; set; }
        public int LocationFireHydrantId { get; set; }
        public virtual LocationFireHydrant Locations { get; set; }
    }
}
