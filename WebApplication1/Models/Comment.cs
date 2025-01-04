using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication1.Models
{
    public class Comment
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//primary key, identity = yes
        [DisplayName("ID")]
        public int CommentID { get; set; }



        [DisplayName("Kullanıcı")]
        public int UserID { get; set; }


        [DisplayName("ProductID")]
        public int ProductID { get; set; } // Normalizasyon gerektirmez


        [Range(10, 150)] //Stringlenght en az 10 en çok 150 karakter
        [DisplayName("Yorum")]
        public string? Review { get; set; }

    }
}
