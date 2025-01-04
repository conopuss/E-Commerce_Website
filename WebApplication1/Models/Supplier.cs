using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public class Supplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//primary key, identity = yes
        [DisplayName("ID")]
        public int SupplierID { get; set; }

        [StringLength(100, ErrorMessage = "En fazla 100 karakter ")]
        [DisplayName("Marka Adı")] //Formda, label'da görünecek metin
        [Required(ErrorMessage = "Marka Adı Zorunlu Alan")]
        //Regular expression
        public string? BrandName { get; set; }


        [DisplayName("Resim Seç")]
        [Required(ErrorMessage = "Resim girmek zorundasınız ")]
        public string? PhotoPath { get; set; }


        [DisplayName("Aktif/Pasif")]
        public bool Active { get; set; }

    }
}
