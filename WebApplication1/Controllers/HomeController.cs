using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class HomeController : Controller
	{
		public static string OrderGroupGUID = "";

		Baglanti context = new Baglanti();

		MainPageModel mpm = new MainPageModel();
		Cls_Product cls_Product = new Cls_Product();
		Cls_Order cls_Order = new Cls_Order();
		Cls_Category cls_Category = new Cls_Category();
		Cls_Supplier cls_Supplier = new Cls_Supplier();
		Cls_User cls_User = new Cls_User();


		public HomeController()
		{
			cls_Product.mainpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).MainPageCount;
			cls_Product.subpageCount = context.Settings.FirstOrDefault(s => s.SettingID == 1).SubpageCount;
		}
		public async Task<IActionResult> Index()
		{
			mpm.SliderProducts = cls_Product.ProductSelect("Slider", -1);
			mpm.NewProducts = cls_Product.ProductSelect("New", -1);
			mpm.SpecialProducts = cls_Product.ProductSelect("Special", -1);
			mpm.DiscountedProducts = cls_Product.ProductSelect("Discounted", -1);
			mpm.HighlightedProducts = cls_Product.ProductSelect("Highlighted", -1);
			mpm.TopsellerProducts = cls_Product.ProductSelect("Topseller", -1);
			mpm.OpportunityProducts = cls_Product.ProductSelect("Opportunity", -1);
			mpm.NotableProducts = cls_Product.ProductSelect("Notable", -1);


			return View(mpm);

		}
		public IActionResult CartProcess(int id)
		{
			string refererUrl = Request.Headers["Referer"].ToString();
			string url = "";

			if (id > 0)
			{
				Cls_Product.Highlighted_Increase(id);

				cls_Order.ProductID = id;
				cls_Order.Quantity = 1;

				var cookieOptions = new CookieOptions();
				var cookie = Request.Cookies["sepetim"];
				if (cookie == null)
				{
					cookieOptions.Expires = DateTime.Now.AddDays(1);
					cookieOptions.Path = "/";

					cls_Order.MyCart = "";
					cls_Order.AddToMyCart(id.ToString());

					Response.Cookies.Append("sepetim", cls_Order.MyCart, cookieOptions);
					TempData["success"] = "Ürün sepetinize eklendi";
				}
				else
				{
					cls_Order.MyCart = cookie; // tarayıcıdaki sepetim içerisindeki daha önceki ürünleri propery'e gönderdim.
					if (cls_Order.AddToMyCart(id.ToString()) == false)
					{
						HttpContext.Response.Cookies.Append("sepetim", cls_Order.MyCart, cookieOptions);
						cookieOptions.Expires = DateTime.Now.AddDays(1);

						TempData["success"] = "Ürün sepetinize eklendi";
					}
					else
					{
						TempData["error"] = "Bu ürün zeten sepetinizde var";
					}
				}

				Uri refererUri = new Uri(refererUrl, UriKind.Absolute);
				url = refererUri.AbsolutePath;
				if (url.Contains("DpProducts") || refererUrl.Contains("http://localhost:7210"))
				{
					return RedirectToAction("Index");
				}
				return Redirect(url);
			}
			else
			{
				// Handle cases where id is not greater than 0
				Uri refererUri = new Uri(refererUrl, UriKind.Absolute);
				url = refererUri.AbsolutePath; // Get the path part of the URL

				if (url.Contains("DpProducts"))
				{
					return RedirectToAction("Index");
				}
				return Redirect(url);
			}
		}


		public IActionResult Cart() // bu dosyanın cshtml dosyası var!!
		{


			//sağ üst köşedem buraya geldim
			if (HttpContext.Request.Query["ProductID"].ToString() == "")
			{
				var cookie = Request.Cookies["sepetim"]; // kullanıcı sepete ürün eklemeden sepet detayını göster diye tıklarsa 
				if (cookie == null)
				{
					cls_Order.MyCart = "";

					ViewBag.Sepetim = cls_Order.SelectMyCart(); //sepeti listele ama içinde kayıt yok;


				}
				else // sağ üst köşeden tıkladığında sepette en az bir ürün varsa
				{
					var cookieOptions = new CookieOptions();
					cls_Order.MyCart = Request.Cookies["sepetim"];
					ViewBag.Sepetim = cls_Order.SelectMyCart(); //sepeti listele;

				}
			}
			else
			{
				string? ProductID = HttpContext.Request.Query["ProductID"]; // html sayfasındaki ID yi metotta parametre olarak değil  sorgu parametresinden yakaladık
				cls_Order.MyCart = Request.Cookies["sepetim"]; // tarayıcıdan al, property'e yaz
				cls_Order.DeleteFromMyCart(ProductID); //silme metodunu çağırdım,silme detodu property'i kullanarak silme işlemi yaptı

				var cookieOptions = new CookieOptions();
				Response.Cookies.Append("sepetim", cls_Order.MyCart, cookieOptions);
				cookieOptions.Expires = DateTime.Now.AddDays(1); // 1 günlük çerez
				TempData["success"] = "Ürün Sepetten Silindi";


				ViewBag.Sepetim = cls_Order.SelectMyCart();//sepeti listele
			}



			return View();
		}

		[HttpGet]
		public IActionResult Order()
		{
			if (HttpContext.Session.GetString("Email") != null)
			{
				User? usr = Cls_User.SelectMemberInfo(HttpContext.Session.GetString("Email"));
				if (usr != null)
				{
					return View(usr);
				}
				return RedirectToAction("Login");
			}
			else
			{
				return RedirectToAction("Login");

			}

		}

		[HttpPost]
		public IActionResult Order(IFormCollection frm)
		{
			string txt_individual = Request.Form["txt_individual"];
			string txt_corporate = Request.Form["txt_corporate"];
			if (txt_individual != null)
			{
				//bireysel fatura
				//digital planet
				//WebServiceController.tckimlik_vergi_no = txt_individual;
				//Cls_Order.tckimlik_vergi_no = txt_individual;
				//cls_Order.EfaturaCreate();
			}
			else
			{
				//kurumsal fatura
				//WebServiceController.tckimlik_vergi_no = txt_corporate;
				//Cls_Order.tckimlik_vergi_no = txt_corporate;
				//cls_Order.EfaturaCreate();
			}



			string kredikartno = Request.Form["kredikartno"];
			string kredikartay = frm["kredikartay"];
			string kredikartyil = frm["kredikartyil"];
			string kredikartcvs = frm["kredikartcvs"];



			return RedirectToAction("backref");
			//buradan sonraki kodlar , payu , iyzico


			//payu dan gelen örnek kodlar


			/* AŞAGIDAKİ KODLAR GERÇEK HAYATTA AÇILALAK 

            NameValueCollection data = new NameValueCollection();
            string url = "https://www.firma.com/backref";



            data.Add("BACK_REF", url);
            data.Add("CC_CVV", kredikartcvs);
            data.Add("CC_NUMBER", kredikartno);
            data.Add("EXP_MONTH", kredikartay);
            data.Add("EXP_YEAR", "20" + kredikartyil);



            var deger = "";

                foreach (var item in data)
                {
                    var value = item as string;
                    var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
                    deger += byteCount + data.Get(value);
                }



                var signatureKey = "size verilen SECRET_KEY buraya yazılacak";



                var hash = HashWithSignature(deger, signatureKey);



                data.Add("ORDER_HASH", hash);



                var x = POSTFormPAYU("https://secure.payu.com.tr/order/....", data);



                //sanal kart
                if (x.Contains("<STATUS>SUCCESS</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
                {
                    //sanal kart (debit kart) ile alış veriş yaptı , bankadan onay aldı
                }
                else
                {
                    //gerçek kart ile alış veriş yaptı , bankadan onay aldı
                }
                */


		}

		public static string HashWithSignature(string deger, string signatureKey)
		{
			return "";
		}



		public static string POSTFormPAYU(string url, NameValueCollection data)
		{
			return "";
		}

		public IActionResult backref()
		{
			//sipariş tablosuna kaydet
			//sepetim cookie sinden sepeti temizleyecegiz
			//e-fatura olustur metodunu cagır
			var cookieOptions = new CookieOptions();
			var cookie = Request.Cookies["sepetim"];
			if (cookie != null)
			{
				cls_Order.MyCart = cookie;
				OrderGroupGUID = cls_Order.OrderCreate(HttpContext.Session.GetString("Email").ToString());

				cookieOptions.Expires = DateTime.Now.AddDays(1);
				Response.Cookies.Delete("sepetim"); //tarayıcıdan sepeti sil
													//    cls_User.Send_Sms(OrderGroupGUID);
													//   cls_User.Send_Email(OrderGroupGUID);
			}
			return RedirectToAction("ConfirmPage");
		}

		public IActionResult ConfirmPage()
		{

			ViewBag.OrderGroupGUID = OrderGroupGUID;
			return View();
		}



		public IActionResult MyOrders()
		{
			if (HttpContext.Session.GetString("Email") != null)
			{
				List<Vw_MyOrder> orders = cls_Order.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
				return View(orders);
			}
			else
			{
				return RedirectToAction(nameof(Login));
			}

		}



		[HttpGet]
		[Route("Home/Register")]
		public IActionResult Register()
		{
			HttpContext.Session.Clear();
			return View();
		}

		[HttpPost]
		public IActionResult Register(User user)
		{

			if (ModelState.IsValid)
			{
				bool usr = cls_User.loginControl(user.Email);
				if (usr == false)
				{
					bool answer = Cls_User.AddUser(user);

					if (answer)
					{
						TempData["success"] = "Kaydedildi";
						return RedirectToAction("Login");
					}
					TempData["error"] = "Hata! Tekrar deneyiniz.";
				}
				else
				{
					TempData["error"] = "Bu Email Zaten mevcut.Başka Deneyiniz";
				}
			}

			return View();

		}

		[HttpGet]
		public IActionResult UserEdit(int id)
		{


			if (id == null || context.Users == null)
			{
				return NotFound();
			}
			var user = cls_User.UserDetails(id);
			return View(user);
		}
		[HttpPost]
		public IActionResult UserEdit(User user)
		{


			bool answer = Cls_User.UserUpdate(user);
			if (answer)
			{
				TempData["success"] = "Güncellendi!";
				return RedirectToAction(nameof(Order));
			}
			else
			{
				TempData["error"] = "HATA!";
				return RedirectToAction(nameof(UserEdit));
			}
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(User user)
		{
			string answer = Cls_User.loginControl(user);
			if (answer == "error")
			{
				TempData["error"] = "Email/Şifre yanlış girildi";
				return View();
			}
			else if (answer.Contains("@"))
			{
				// Get user information
				using (Baglanti context = new Baglanti())
				{
					var usr = context.Users.FirstOrDefault(u => u.Email == user.Email);
					if (usr != null)
					{

						HttpContext.Session.SetInt32("UserID", usr.UserID);
						HttpContext.Session.SetString("Email", usr.Email);
						HttpContext.Session.SetString("Telephone", usr.Telephone); // Store Telephone
						HttpContext.Session.SetString("NameSurname", usr.NameSurname);
					}
				}
				return RedirectToAction("Index");
			}
			else
			{
				HttpContext.Session.SetString("Email", answer);
				HttpContext.Session.SetString("Admin", answer);

				return RedirectToAction("Index", "Admin");
			}
		}
		public IActionResult Logout()
		{
			//HttpContext.Session.Remove("Admin");
			//HttpContext.Session.Remove("Email");
			HttpContext.Session.Clear();
			return RedirectToAction("Index");
		}

		public IActionResult NewProducts()
		{
			//ViewBag.PageTotal = Cls_Product.ProductCount();
			mpm.NewProducts = cls_Product.ProductSelect("New", 0); //yeni
			return View(mpm);
		}

		public PartialViewResult _PartialNewProducts(string pageno)
		{
			//cls_Product.subpageCount = subpageCount; // property
			int pagenumber = Convert.ToInt32(pageno);
			mpm.NewProducts = cls_Product.ProductSelect("New", pagenumber); //yeni
			return PartialView(mpm);
		}




		public IActionResult SpecialProducts()
		{
			//cls_Product.subpageCount = subpageCount;
			mpm.SpecialProducts = cls_Product.ProductSelect("Special", 0);
			return View(mpm);
		}
		public PartialViewResult _PartialSpecialProducts(string pageno)
		{
			int pagenumber = Convert.ToInt32(pageno);
			mpm.SpecialProducts = cls_Product.ProductSelect("Special", pagenumber);
			return PartialView(mpm);
		}

		public IActionResult DiscountedProducts()
		{
			//cls_Products.subpageCount = subpageCount;
			mpm.DiscountedProducts = cls_Product.ProductSelect("Discounted", 0);
			return View(mpm);
		}

		public PartialViewResult _PartialDiscountedProducts(string pageno)
		{
			int pagenumber = Convert.ToInt32(pageno);
			mpm.DiscountedProducts = cls_Product.ProductSelect("Discounted", pagenumber);
			return PartialView(mpm);
		}

		public IActionResult HighlightedProducts()
		{
			//cls_Products.subpageCount = subpageCount;
			mpm.HighlightedProducts = cls_Product.ProductSelect("Highlighted", 0);
			return View(mpm);
		}

		public PartialViewResult _PartialHighlightedProducts(string pageno)
		{
			int pagenumber = Convert.ToInt32(pageno);
			mpm.HighlightedProducts = cls_Product.ProductSelect("Highlighted", pagenumber);
			return PartialView(mpm);
		}

		public IActionResult TopsellerProducts()
		{

			//cls_Products.subpageCount = subpageCount;
			mpm.TopsellerProducts = cls_Product.ProductSelect("Topseller", 0);
			return View(mpm);
		}

		public PartialViewResult _PartialTopsellerProducts(string pageno)
		{
			int pagenumber = Convert.ToInt32(pageno);
			mpm.TopsellerProducts = cls_Product.ProductSelect("Topseller", pagenumber);
			return PartialView(mpm);
		}

		public IActionResult OpportunityProducts()
		{
			//cls_Product.subpageCount = subpageCount;
			mpm.OpportunityProducts = cls_Product.ProductSelect("Opportunity", 0);
			return View(mpm);
		}
		public PartialViewResult _PartialOpportunityProducts(string pageno)
		{
			int pagenumber = Convert.ToInt32(pageno);
			mpm.OpportunityProducts = cls_Product.ProductSelect("Opportunity", pagenumber);
			return PartialView(mpm);
		}

		public IActionResult NotableProducts()
		{
			//cls_Product.subpageCount = subpageCount;
			mpm.NotableProducts = cls_Product.ProductSelect("Notable", 0);
			return View(mpm);
		}
		public PartialViewResult _PartialNotableProducts(string pageno)
		{
			int pagenumber = Convert.ToInt32(pageno);
			mpm.NotableProducts = cls_Product.ProductSelect("Notable", pagenumber);
			return PartialView(mpm);
		}



		public async Task<IActionResult> CategoryPage(int id)
		{

			List<Product> products = cls_Product.ProductSelect(id, "Category");

			Category category = await cls_Category.CategoryDetails(id);
			ViewBag.Header = category.CategoryName;
			return View(products);
		}

		public IActionResult SupplierPage(int id)
		{
			List<Product> products = cls_Product.ProductSelect(id, "Supplier");

			Supplier supplier = cls_Supplier.SupplierDetails(id);
			ViewBag.Header = supplier.BrandName;
			return View(products);

		}


		public IActionResult DetailedSearch()
		{

			ViewBag.Categories = context.Categories.Where(c=> c.Active == true && c.ParentID != 0 ).ToList();
			ViewBag.Suppliers = context.Suppliers.Where(c => c.Active == true).ToList();



			return View();
		}

		public PartialViewResult _DProducts()
		{


			return PartialView();
		}
		[HttpPost]
		public IActionResult DProducts(int CategoryID, string[] SupplierID, string price, string IsInStock)
		{
			if(CategoryID == 0 || SupplierID.Length == 0 || SupplierID == null)
			{
				TempData["error"] = "";
				return View();
			}


			price = price.Replace(" ", "").Replace("TL", "");
			string[] PriceArray = price.Split('-');
			string startprice = PriceArray[0];
			string endprice = PriceArray[1];

			string sign = ">";
			if (IsInStock == "0")
			{
				sign = ">=";
			}

			string suppliervalue = ""; //1,2,4
			for (int i = 0; i < SupplierID.Length; i++)
			{
				if (i == 0)
				{
					//ilk marka
					suppliervalue = "SupplierID =" + SupplierID[i];
				}
				else
				{
					//ikinci ve sonrası markalar
					suppliervalue += " or SupplierID =" + SupplierID[i];
				}
			}

			string query = "select * from Products where CategoryID=" + CategoryID + " and (" + suppliervalue + ") and (UnitPrice >= " + startprice + " and UnitPrice <= " + endprice + ") and Stock " + sign + " 0 order by UnitPrice";

			
			var products = cls_Product.SelectProductByDetails(query);
		
			ViewBag.Products = products;

			return PartialView("_DProducts");
		}

		[HttpGet]
		public IActionResult Details(int id)
		{
			Cls_Product.Highlighted_Increase(id);

			mpm.ProductDetails = context.Products.FirstOrDefault(p => p.ProductID == id);
			mpm.CategoryName = (from p in context.Products
								join c in context.Categories
							  on p.CategoryID equals c.CategoryID
								where p.ProductID == id
								select c.CategoryName).FirstOrDefault();
			mpm.BrandName = (from p in context.Products
							 join s in context.Suppliers
						   on p.SupplierID equals s.SupplierID
							 where p.ProductID == id
							 select s.BrandName).FirstOrDefault();

			mpm.RelatedProducts = context.Products.Where(p => p.Related == mpm.ProductDetails!.Related && p.ProductID != id).ToList();
			ViewBag.Notes = context.Products.Where(p => p.ProductID == id).Select(p => p.Notes).FirstOrDefault();

			return View(mpm);
		}




		[HttpPost]
		public PartialViewResult gettingSearch(string id)
		{

			id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
			List<Sp_Search> ulist = Cls_Product.gettingSearchProducts(id);
			string json = JsonConvert.SerializeObject(ulist);
			var response = JsonConvert.DeserializeObject<List<Search>>(json);
			return PartialView(response);
		}

		[HttpGet]
		public IActionResult ContactUs()
		{
			// Retrieve session data
			var userEmail = HttpContext.Session.GetString("Email");
			var userID = HttpContext.Session.GetInt32("UserID");
			var userTelephone = HttpContext.Session.GetString("Telephone");

			// Check if user is logged in and UserID exists
			if (!string.IsNullOrEmpty(userEmail) && userID.HasValue)
			{
				// Retrieve user data based on UserID
				using (var context = new Baglanti())
				{
					var user = context.Users.FirstOrDefault(u => u.UserID == userID.Value);

					if (user != null)
					{
						// Pre-fill the model for the view
						user.Message = "";
						return View(user);
					}
				}
			}

			// If user is not logged in, redirect to login page
			return RedirectToAction(nameof(Login));
		}

		[HttpPost]
		public IActionResult ContactUs(User user)
		{
			if (ModelState.IsValid)
			{
				// Update the user's message based on UserID
				using (var context = new Baglanti())
				{
					var existingUser = context.Users.FirstOrDefault(u => u.UserID == user.UserID);
					if (existingUser != null)
					{
						// Update the message and save
						existingUser.Message = user.Message;
						context.SaveChanges();

						TempData["success"] = "Değerlendirmeniz alındı";


						return View(existingUser);
					}
				}
			}

			return View(user);
		}

		[HttpGet]
		public IActionResult AboutUs()
		{
			return View();
		}

	}








}

