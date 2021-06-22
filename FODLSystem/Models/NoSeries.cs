using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models
{
    public class NoSeries
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string LastNoUsed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
