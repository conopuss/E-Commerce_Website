using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public class Status
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//primary key, identity = yes
        [DisplayName("ID")]
        public int StatusID { get; set; }

        [StringLength(100)]
        [Required]
        public string? StatusName { get; set; } = string.Empty;


        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }

    }
}
