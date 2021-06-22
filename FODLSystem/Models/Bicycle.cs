using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models
{
    public class Bicycle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
     
        [Display(Name = "Name of Owner")]
        [Required]
        public string NameOwner { get; set; }
        [Display(Name = "Contact No")]
       
        public string ContactNo { get; set; }
        [Display(Name = "Brand Name")]
        [Required]
        public string BrandName { get; set; }
        //public DateTime DateInspected { get; set; }
        //public DateTime DateExpiry { get; set; }

        [Display(Name ="Identification No")]
        [Required]
        public string IdentificationNo { get; set; }
        public string Status { get; set; } = "Active";
        [Display(Name = "Department Name")]
        public int DepartmentID { get; set; }
        public virtual Department Departments { get; set; }
    }
}
