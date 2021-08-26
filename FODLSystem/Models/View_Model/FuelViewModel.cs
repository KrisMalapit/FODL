using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models.View_Model
{
    public class FuelViewModel
    {
        
        public string ReferenceNo { get; set; }
        public string Shift { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int DispenserId { get; set; }
        public int LubeTruckId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; } 
    }
    //public class FuelOilViewModel
    //{
    //}
    public class FuelOilDetailViewModel
    {
        public int EquipmentId { get; set; }
        
        public int LocationId { get; set; }
      
        public string SMR { get; set; }

        public string Signature { get; set; }
       
       
        public DateTime CreatedDate { get; set; }
        public int FuelOilId { get; set; }
        public string Status { get; set; }

    }
    public class FuelOilSubDetailViewModel
    {
        public DateTime TimeInput { get; set; }
      
        public int ItemId { get; set; }
       
        public int ComponentId { get; set; }
       
        public int VolumeQty { get; set; }
        public int FuelOilDetailId { get; set; }
      
        public string Status { get; set; }
    }
    
}
