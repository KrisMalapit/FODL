using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models.View_Model
{
    public class FuelOilViewModel
    {
        public string ReferenceNo { get; set; }
        public string Shift { get; set; }
       
        public int EquipmentId { get; set; }
        public int LocationId { get; set; }
        public int DispenserId { get; set; }
       
        public int LubeTruckId { get; set; }
        public string SMR { get; set; }
        public int[] detail_id { get; set; }
        public string[] no { get; set; }
        public string[] component { get; set; }
        public string[] volume { get; set; }
        public string Signature { get; set; }

    }
}
