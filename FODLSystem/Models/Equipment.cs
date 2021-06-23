using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR(50)")]
        [Required]
        public string No { get; set; }
        public string Name { get; set; }
        [Display(Name = "Model No")]
        public string ModelNo { get; set; }

        [Display(Name = "DepartmentCode")]
        public string DepartmentCode { get; set; }
        [Display(Name = "Fuel Code Diesel")]
        public string FuelCodeDiesel { get; set; }
        [Display(Name = "Fuel Code Oil")]
        public string FuelCodeOil { get; set; }
       

        public string Status { get; set; } = "Active";

    }
}
