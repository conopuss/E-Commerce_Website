using System.ComponentModel;

namespace WebApplication1.Models
{
	public class Activity
	{

        [DisplayName("Tür")]
        public string? Tur { get; set; }


        [DisplayName("Adı")]
        public string? Adi { get; set; }

        [DisplayName("Başlangıç")]
        public DateTime EtkinlikBaslamaTarihi { get; set; }


        [DisplayName("Bitiş")]
        public DateTime EtkinlikBitisTarihi { get; set; }


        [DisplayName("Yer")]
        public string? EtkinlikMerkezi { get; set; }


        [DisplayName("Açıklama")]
        public string? KisaAciklama { get; set; }


        [DisplayName("Bilet")]
        public string? BiletSatisLinki { get; set; }


        [DisplayName("Afiş")]
        public string? KucukAfis { get; set; }

	}
}
