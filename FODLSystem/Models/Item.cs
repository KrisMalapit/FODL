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
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Serial No")]
        public string SerialNo { get; set; }
        [Display(Name = "Date Purchased")]
        public DateTime? DatePurchased { get; set; }
        public int? Warranty { get; set; }
        [Display(Name = "Item Status")]
        public string ItemStatus { get; set; } 
        public string Status { get; set; } = "Active";
        [Display(Name = "Equipment Type")]
        public string EquipmentType { get; set; }
        public int IsIn { get; set; } = 1;
       
        public string Type { get; set; }
       
        public string Capacity { get; set; }
    }
}
