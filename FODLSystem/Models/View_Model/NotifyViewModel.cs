using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models.View_Model
{
    public class NotifyViewModel
    {
        public int CompanyId { get; set; }
        public string ReferenceNo { get; set; }
        public string Area { get; set; }
        public string Equipment { get; set; }
        public string DocumentStatus { get; set; }
        public string Location { get; set; }
    }
}
