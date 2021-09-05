using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR(20)")]
        [Required]
        public string No { get; set; }
      
        [Required]
        public string Description { get; set; }
        [Display(Name = "Description 2")]
       
        public string Description2 { get; set; }
        [Display(Name = "Type of Fuel")]
      
        public string TypeFuel { get; set; }
        [Display(Name = "Description in Liquidation")]
       
        public string DescriptionLiquidation{ get; set; }
        public string Status { get; set; } = "Active";
        public DateTime? DateModified { get; set; }
    }
}
