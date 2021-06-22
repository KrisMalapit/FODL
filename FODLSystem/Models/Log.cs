using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Descriptions { get; set; }
        public string Action { get; set; }
        public string Status { get; set; } 
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
