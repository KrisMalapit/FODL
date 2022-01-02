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
        [Display(Name = "Unit No")]

        public int EquipmentId { get; set; }
        public virtual Equipment Equipments { get; set; }
        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public virtual Location Locations { get; set; }
        public string SMR { get; set; }

        public string Signature { get; set; }
        public string Status { get; set; } = "Active";
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        public int FuelOilId { get; set; }
        public virtual FuelOil FuelOils { get; set; }
        public int OldId { get; set; }
        public int? DriverId { get; set; }
        public virtual Driver Drivers { get; set; }

    }
}
