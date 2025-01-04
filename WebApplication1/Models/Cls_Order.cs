namespace WebApplication1.Models
{
	public class Cls_Order
	{
		public int ProductID { get; set; }
		public int Quantity { get; set; }
		public string MyCart { get; set; }
		public decimal UnitPrice { get; set; }
		public string? ProductName { get; set; }
		public int KDV { get; set; }
		public string? PhotoPath { get; set; }

		Baglanti context = new Baglanti();

		public bool AddToMyCart(string id)
		{
			bool exists = false; // bu metod bittiğinde hala false ise ürün sepete eklendi
			if (MyCart == "") // sepete ilk defa ürün ekleyeceğiz
			{
				MyCart = id + "=1";
			}
			else  //sepette daha önceden eklenmiş ürün veya ürünler var
			{
				string[] MyCartArray = MyCart.Split('&'); // diziyi "&" işaretiyle ayır

				// for içinde aynı ürün septte var mı diye kontrol ediyoruz
				for (int i = 0; i < MyCartArray.Length; i++)
				{
					// 1. Dönüş 10 = 1
					// 2. Dönüş 20 = 1  ...
					string[] MyCartArrayLoop = MyCartArray[i].Split('=');
					if (MyCartArrayLoop[0] == id)
					{
						//aynı ürünü sepete eklemeye çalışıyor
						exists = true;
					}

				}
				if (exists == false) // ürün daha önce sepete eklenmemiş
				{
					MyCart = MyCart + '&' + id.ToString() + "=1";
				}

			}
			return exists;



		}

		public void DeleteFromMyCart(string? id)
		{
			
			string[] MyCartArray = MyCart.Split('&'); //ürünler birbirinden ayrıldı
			string NewMyCart = "";
			int count = 1;

			//ProductID ile adet ayrıldı
			for (int i = 0; i < MyCartArray.Length; i++)
			{

				string[] MyCartArrayLoop = MyCartArray[i].Split('=');
				
				string MyCartID = MyCartArrayLoop[0];
				if (MyCartID != id)
				{
					//10=1&20=1&40=1
					if (count == 1)
					{
						//yeni sepetin içine ilk ürünü ekliyorum, '&' yok
						NewMyCart = MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
						count++;
					}
					else
					{
						//yeni sepetin içinde daha önce silinmeyecek olan ürün/ürünler var, '&' productId'nin önüne ekliyorum
						NewMyCart = NewMyCart + '&' + MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
					}
				}
			}
			MyCart = NewMyCart; //

		}
		public List<Cls_Order> SelectMyCart()
		{
			//List<Cls_Order> , sepet bilgilerini property'lere koyacağım. Property üzerinden dönüş yapacak
			List<Cls_Order> list = new List<Cls_Order>();
			string[] MyCartArray = MyCart.Split('&');

			if (MyCartArray[0] != "")
			{
				//eğer sepette son ürün silinecek olursa geriye ürün kalmayacağı için aşağıda yazacağım kodlar patlamasın diye	"" kontrolü yapıyorum
				for (int i = 0; i < MyCartArray.Length; i++)
				{
					string[] MyCartArrayLoop = MyCartArray[i].Split('=');
					int MyCartID = Convert.ToInt32(MyCartArrayLoop[0]);

					Product? prd = context.Products.FirstOrDefault(p => p.ProductID == MyCartID);
					// prd içinde veritabanı kayıtları var bunları propertylere yazdırıyorum

					Cls_Order ord = new Cls_Order();
					ord.ProductID = prd.ProductID;
					ord.Quantity = Convert.ToInt32(MyCartArrayLoop[1]);
					ord.UnitPrice = prd.UnitPrice;
					ord.ProductName = prd.ProductName;
					ord.PhotoPath = prd.PhotoPath;
					ord.KDV = prd.KDV;
					list.Add(ord);

				}
			}
			return list;

		}

		public string OrderCreate(string Email)
		{
			List<Cls_Order> sipList = SelectMyCart();
			string OrderGroupGUID = DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace(".", "");
			DateTime OrderDate = DateTime.Now;	
			foreach(var item in sipList)
			{
				Order order = new Order();
				order.OrderDate = OrderDate;
				order.OrderGroupGUID = OrderGroupGUID;
				order.UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;
				order.ProductID = item.ProductID;
				order.Quantity = item.Quantity;
				context.Orders.Add(order);
				context.SaveChanges();
			}
			return OrderGroupGUID;
		}

		public List<Vw_MyOrder>SelectMyOrders(string Email)
		{
			int UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;
			List<Vw_MyOrder> myOrders = context.Vw_MyOrders.Where(o=> o.UserID == UserID).ToList();
			return myOrders;
		}





	}
}
