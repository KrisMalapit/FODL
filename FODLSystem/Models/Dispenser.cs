using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class Dispenser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR(20)")]
        [Required]
        public string No { get; set; }
        [Column(TypeName = "VARCHAR(100)")]
        [Required]
        public string Name { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime? DateModified { get; set; }
    }
}
