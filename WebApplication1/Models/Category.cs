using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//primary key, identity = yes
        [DisplayName("ID")]
        public int CategoryID { get; set; }


        [StringLength(50, ErrorMessage = "En fazla 50 karakter ")]
        [DisplayName("Kategori Adı")] //Formda, label'da görünecek metin
        [Required(ErrorMessage = "Kategori Adı Zorunlu Alan")]
        public string? CategoryName { get; set; }

        [DisplayName("Üst Kategori")]
        public int? ParentID { get; set; } //Üst ID

        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }

    }
}
