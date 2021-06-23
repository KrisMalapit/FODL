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
      
    }
}
