using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models.View_Model
{
    public class InventoryViewModel
    {
        public string Code { get; set; }
        public string ItemName { get; set; }
        public string SerialNo { get; set; }
        public DateTime? DatePurchased { get; set; }
        public string ItemStatus { get; set; }
        public string EquipmentType { get; set; }
        public string Location { get; set; }
        public string Area { get; set; }
        public int? Warranty { get; set; }

    }
}
