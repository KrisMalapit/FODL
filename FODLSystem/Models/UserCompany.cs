using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models
{
    public class UserCompany
    {
        public int UserId { get; set; }
        public virtual User Users { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
