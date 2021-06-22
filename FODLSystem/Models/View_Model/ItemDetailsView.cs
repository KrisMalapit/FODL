using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models.View_Model
{
    public class ItemDetailsView
    {
        public int MDDAFId { get; set; }

        public int ItemId { get; set; }
        public int QuantityOrdered { get; set; }
        public decimal QuantityTotalReceived { get; set; }
        public decimal QuantityReceived { get; set; }

        public string haveRecord { get; set; }
    }
}
