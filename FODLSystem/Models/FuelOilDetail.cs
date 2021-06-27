using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class FuelOilDetail
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
        public int FuelOilId { get; set; }
        public virtual FuelOil FuelOils { get; set; }
        public string Status { get; set; } = "Active";
    }
}
