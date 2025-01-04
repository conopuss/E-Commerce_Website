using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
	public class Cls_Product
	{
		public int mainpageCount { get; set; }
		public int subpageCount { get; set; }
		public int page { get; set; }
		public int ProductID { get; set; }
		public string? ProductName { get; set; }
		public decimal UnitPrice { get; set; }
		public string? PhotoPath { get; set; }
		public string? Notes { get; set; }

		Baglanti context = new Baglanti();

        //metod overload (aynı isimli metodu , farklı parametre sırasıyla cagırmak)
        public List<Product> ProductSelect(string mainPageName, int pagenumber)
        {
            List<Product> products;
            if (mainPageName == "Slider")
            {
                products = context.Products.Where(p => p.StatusID == 1).Take(mainpageCount).ToList();
            }
            else if (mainPageName == "New")
            {
                if (pagenumber < 0)
                {
                    //Home/Index
                    products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Take(mainpageCount).ToList();
                }
                else
                {
                    //Yukarıdaki yeni ürünler menüsü - alt sayfa tıklandığında
                    if (pagenumber == 0)
                    {
                        // sayfaya ilk tıklama
                        products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Take(subpageCount).ToList();
                    }
                    else
                    {
                        //ajax
                        products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.AddDate).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                    }


                }

            }


            else if (mainPageName == "Special")
            {
				if (pagenumber< 0)
				{
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).OrderBy(p => p.ProductName).Take(mainpageCount).ToList();
                }
				else
				{
					if(pagenumber == 0)
					{
						products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).OrderBy(p=> p.ProductName).Take(subpageCount).ToList();
					}
					else
					{
						products = context.Products.Where(p => p.StatusID == 2 && p.Active == true).OrderBy(p => p.ProductName).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
					}
				}
                
            }
            else if (mainPageName == "Discounted")
            {
				if (pagenumber < 0)
				{
					products = context.Products.OrderByDescending(p => p.Discount).Take(mainpageCount).ToList();
				}
				else
				{
					if(pagenumber == 0)
					{
						products = context.Products.Where(p=> p.Active== true).OrderByDescending(p=> p.Discount).Take(subpageCount).ToList();
					}
					else
					{
						products = context.Products.Where(p=> p.Active == true).OrderByDescending(p=> p.Discount).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
					}
				}
            }
            else if (mainPageName == "Highlighted")
            {
				if (pagenumber < 0)
				{
					products = context.Products.OrderByDescending(p => p.HighLighted).Take(mainpageCount).ToList();
				}
				else
				{
					if (pagenumber == 0)
					{
						products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Take(subpageCount).ToList();
					}
					else
					{
						products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.HighLighted).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
					}
				}
            }
            else if (mainPageName == "Topseller")
            {
                
                if (pagenumber < 0)
                {
                    products = context.Products.OrderByDescending(p => p.TopSeller).Take(mainpageCount).ToList();
                }
                else
                {
                    if (pagenumber == 0)
                    {
                        products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.TopSeller).Take(subpageCount).ToList();
                    }
                    else
                    {
                        products = context.Products.Where(p => p.Active == true).OrderByDescending(p => p.TopSeller).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                    }
                }
            }
           
            else if (mainPageName == "Opportunity")
            {
				if (pagenumber < 0)
				{
					products = context.Products.Where(p => p.StatusID == 4).ToList();

				}
				else
				{
					if (pagenumber == 0)
					{
						products = context.Products.Where(p => p.StatusID == 4 && p.Active == true).OrderBy(p => p.ProductName).Take(subpageCount).ToList();
					}
					else
					{
						products = context.Products.Where(p => p.StatusID == 4 && p.Active == true).OrderBy(p => p.ProductName).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
					}
				}
            }
            else if (mainPageName == "Notable")
            {
				if (pagenumber < 0)
				{
					products = context.Products.Where(p => p.StatusID == 5).ToList();
				}
                else
                {
                    if (pagenumber == 0)
                    {
                        products = context.Products.Where(p => p.StatusID == 5 && p.Active == true).OrderBy(p => p.ProductName).Take(subpageCount).ToList();
                    }
                    else
                    {
                        products = context.Products.Where(p => p.StatusID == 5 && p.Active == true).OrderBy(p => p.ProductName).Skip(pagenumber * subpageCount).Take(subpageCount).ToList();
                    }
                }
            }

            else
            {
                products = context.Products.ToList();
            }

            return products;
        }





        //List<Product> products = cls_Product.ProductSelect(id);
        public List<Product> ProductSelect(int id, string TableName)
		{
			List<Product> products;
			if (TableName == "Category") //ana sayfa,kategori tıklanınca gelecek ürünler
			{
				//select * from Products where CategoryID = id
				products = context.Products.Where(p => p.CategoryID == id).OrderBy(p => p.ProductName).ToList();
			}
			else if (TableName == "Supplier")  //ana sayfa,marka tıklanınca gelecek ürünler
			{
				products = context.Products.Where(p => p.SupplierID == id).OrderBy(p => p.ProductName).ToList();
			}
			else
			{
				products = context.Products.ToList();//admin ürün listele
			}
			return products;
		}

	
		public static bool ProductInsert(Product product)
		{
			try
			{
				using (Baglanti context = new Baglanti())
				{
					product.AddDate = DateTime.Now;
					context.Add(product);
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}


		public Product ProductDetails(int? id)
		{
			Product? product;
			if (id == 0)  
			{
				product = context.Products.Where(p => p.StatusID == 6).FirstOrDefault();
			}
			else
			{
				product = context.Products.Find(id);
			}
			return product;
		}


		public static bool ProductUpdate(Product product)
		{
			try
			{
				using (Baglanti context = new Baglanti())
				{
					context.Update(product);
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}


		public static bool ProductDelete(int id)
		{
			try
			{
				using (Baglanti context = new Baglanti())
				{
					Product? product = context.Products.FirstOrDefault(c => c.ProductID == id);
					product.Active = false;
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}


		//ürünün detayına veya sepete ekle butonuna tıklanınca, Highlighted(öne cıkanlar) kolon değerini bir arttırıyoruz
		public static void Highlighted_Increase(int id)
		{
			using (Baglanti context = new Baglanti())
			{
				Product? product = context.Products.FirstOrDefault(p => p.ProductID == id);
				product.HighLighted += 1;
				context.Update(product);
				context.SaveChanges();
			}
		}


		public static int ProductCount()
		{
			using (Baglanti context = new Baglanti())
			{
				return context.Products.Count();
			}
		}

		public List<Cls_Product> SelectProductByDetails(string query)
        {
            List<Cls_Product> products = new List<Cls_Product>();
            SqlConnection sqlConnection = Connection.ServerConnect;
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                Cls_Product product = new Cls_Product();
                product.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
                product.ProductName = sqlDataReader["ProductName"].ToString();
                product.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
                product.PhotoPath = sqlDataReader["PhotoPath"].ToString();
                product.Notes = sqlDataReader["Notes"].ToString();
                products.Add(product);
            }
            return products;
           
        }
		
		public static List<Sp_Search> gettingSearchProducts(string id)
		{
			Baglanti context = new Baglanti();

			var products = context.Sp_Searches.FromSqlRaw($"sp_arama {id}").ToList();
			return products;

		}

	}
}
