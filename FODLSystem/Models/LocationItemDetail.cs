using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class LocationItemDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Equipment { get; set; }
        public int HeaderId { get; set; }
      
        public int ItemId { get; set; }

        public virtual Item Items { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = "Active";
        public string UpdatedBy { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now.Date;
    }
}
