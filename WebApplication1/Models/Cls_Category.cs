using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
	public class Cls_Category
	{
		//CategoryRepository :ICategoryRepositroy

		Baglanti context = new Baglanti();
		public List<Category> CategorySelect(string main_or_all)
		{
			List<Category> categories;
			if (main_or_all == "all")
			{
				categories = context.Categories.ToList();
			}
			else
			{
				categories = context.Categories.Where(c => c.ParentID == 0).ToList();
			}
			return categories;

		}

		public static bool CategoryInsert(Category category)
		{
			try
			{
				//metod static olduğu için tekrar nesne tanımlıyoruz
				//iakademi40Context context = new iakademi40Context()
				using (Baglanti context = new Baglanti())
				{
					if (category.ParentID == null)
					{
						category.ParentID = 0;
					}
					context.Add(category);
					context.SaveChanges();
					return true;
				}

			}
			catch (Exception)
			{

				return false;
			}
		}

		public async Task<Category> CategoryDetails(int? id)
		{
			Category? category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
			// Category? category = await context.Categories.FindAsync(id);
			return category;
		}

		public static bool CategoryUpdate(Category category)
		{
			Baglanti context = new Baglanti();
			try
			{
				{
					if (category.ParentID == null)
					{
						category.ParentID = 0;

					}

					context.Update(category);
					context.SaveChanges();
					return true;
				}

			}
			catch (Exception)
			{

				return false;
			}
		}

        public static bool CategoryDelete(int id)
        {
            try
            {
                using (Baglanti context = new Baglanti())
                {
                    Category? category = context.Categories.FirstOrDefault(c => c.CategoryID == id);
                    category.Active = false;

                   
                    List<Category> categoryList = context.Categories.Where(c => c.ParentID == id).ToList();
                    foreach (var item in categoryList)
                    {
                         
                        item.Active = false;
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }







    }

}
