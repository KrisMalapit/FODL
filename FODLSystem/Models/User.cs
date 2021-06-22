using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models
{
    public enum Domain
    {
        SEMCALACA, SMCDACON, SEMIRARAMINING
    }


    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR(50)")]

        public string Username { get; set; }
        public int RoleId { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public virtual Role Roles { get; set; }
        public string Email { get; set; }
       
        public string Domain { get; set; }
        public string CompanyAccess { get; set; }
        public string UserType { get; set; }
    }
}
