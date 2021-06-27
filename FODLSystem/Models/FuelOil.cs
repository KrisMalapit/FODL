using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class FuelOil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string Shift { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "Unit No")]
      
        public int EquipmentId { get; set; }
        public virtual Equipment Equipments { get; set; }
        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public virtual Location Locations { get; set; }
        public string SMR { get; set; }
       
        public string Signature { get; set; }
        public string Status { get; set; } = "Active";

    }
}
