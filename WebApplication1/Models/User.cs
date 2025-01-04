using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [StringLength(100)]
        [Required]
        public string? NameSurname { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Email Zorunlu Alan")]
        [EmailAddress(ErrorMessage = "Doğru Email Adresi Girmediniz")]
        public string? Email { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Şifre Zorunlu Alan")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Telephone { get; set; }

        public string? InvoiceAddress { get; set; }

        public string? Message { get; set; }

        public bool IsAdmin { get; set; } //personel mi normal kullanıcı mı
        public bool Active { get; set; }

    }
}
