using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//primary key, identity = yes
        [DisplayName("ID")]
        public int ProductID { get; set; }


        [StringLength(100)]
        [Required(ErrorMessage = "Ürün Adı Zorunlu Alan")]
        [DisplayName("Ürün Adı")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Fiyat Zorunlu Alan")]
        [DisplayName("Fiyat")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Kategori")]
        public int CategoryID { get; set; } //Kategoriden Foreign Key yapmış olduk


        [DisplayName("Marka")]
        public int SupplierID { get; set; }

        [DisplayName("Stok")]
        [Required(ErrorMessage = "Stok Zorunlu Alan")]
        public int Stock { get; set; }

        [DisplayName("İndirim")]
        public int? Discount { get; set; }

        [DisplayName("Statü")]
        public int StatusID { get; set; }


        [DisplayName("Tarih")]
        public DateTime AddDate { get; set; }

        [DisplayName("Anahtar Kelimeler")]
        public string? Keywords { get; set; }

        //Encapsulation = Kapsülleme
        private int _KDV { get; set; }

        public int KDV
        {
            get { return _KDV; }
            set { _KDV = Math.Abs(value); }
        }



        public int HighLighted { get; set; } //Öne çıkanlar

        public int TopSeller { get; set; } //Çok satanlar


        [DisplayName("BunaBakan")]
        public int Related { get; set; }


        [DisplayName("Notlar")]
        public string? Notes { get; set; }


        [DisplayName("Resim Seç")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpeg,jpg,gif", ErrorMessage = "Hatalı dosya tipi")]
        public string? PhotoPath { get; set; }


        [DisplayName("Aktif")]
        public bool Active { get; set; }

    }
}
