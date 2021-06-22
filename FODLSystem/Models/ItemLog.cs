using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class ItemLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ItemId { get; set; }
        //public virtual Item Items { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
    }
}
