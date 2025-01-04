using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }

        [StringLength(30)]
        public string? OrderGroupGUID { get; set; } //sipariş no = 20240223123445 siparişin gün ay yıl saat dakika saniye, replace ile

        public int UserID { get; set; }//inner join Users

        public int ProductID { get; set; } // inner join Products

        public int Quantity { get; set; } //adet

    }
}
