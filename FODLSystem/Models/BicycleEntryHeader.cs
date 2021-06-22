using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class BicycleEntryHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public int BicycleId { get; set; }
        public virtual Bicycle Bicycles { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Status { get; set; } = "Active";
        public string DocumentStatus { get; set; } = "Draft";
        public DateTime ApprovedDate { get; set; }
    }
}
