﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class FuelOil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Reference No")]
        public string ReferenceNo { get; set; }
        public string Shift { get; set; }
        [Display(Name = "Created Date")]
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
        [Display(Name = "Dispenser")]
        public int DispenserId { get; set; }
        public virtual Dispenser Dispensers { get; set; }
        [Display(Name = "Lube Truck")]
        public int LubeTruckId { get; set; }
        public virtual LubeTruck LubeTrucks { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
