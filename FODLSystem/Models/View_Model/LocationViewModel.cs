using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models.View_Model
{
    public class LocationViewModel
    {
        public string Equipment { get; set; }
        public int AreaId { get; set; }
        public int[] detail_id { get; set; }
        public string[] location { get; set; }
        public string[] locationcylinder { get; set; }
        public string[] code { get; set; }
        public string[] type { get; set; }
        public string[] capacity { get; set; }
        public string[] serial { get; set; }
        public string[] area { get; set; }
    }
}
