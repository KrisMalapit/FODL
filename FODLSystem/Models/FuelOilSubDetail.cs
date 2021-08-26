using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class FuelOilSubDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime TimeInput { get; set; }
        [Display(Name = "Item No")]
        public int ItemId { get; set; }
        public virtual Item Items { get; set; }
        [Display(Name = "Component")]
        public int ComponentId { get; set; }
        public virtual Component Components { get; set; }
        public int VolumeQty { get; set; }
        public int FuelOilDetailId { get; set; }
        public virtual FuelOilDetail FuelOilDetails { get; set; }
        public string Status { get; set; } = "Active";
        public int OldId { get; set; }

    }
}
