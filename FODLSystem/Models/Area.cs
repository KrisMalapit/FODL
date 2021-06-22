using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FODLSystem.Models
{
    public class Area
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Display(Name = "Area Name")]
        [Required]
        public string Name { get; set; }
        public string Status { get; set; } = "Active";
        [Display(Name = "Company Name")]
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Companies { get; set; }
    }
}
