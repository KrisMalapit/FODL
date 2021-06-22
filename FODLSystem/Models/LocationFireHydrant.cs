using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FODLSystem.Models
{
    public class LocationFireHydrant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Location { get; set; }
        public string Code { get; set; }

        public int AreaId { get; set; }
        public virtual Area Areas { get; set; }
        public string Status { get; set; }
    }
}
