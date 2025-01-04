using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ViewComponents
{
	public class Brands:ViewComponent
	{
		Baglanti context = new Baglanti();
		public IViewComponentResult Invoke()
		{
			List<Supplier> suppliers = context.Suppliers.Where(c => c.Active == true).ToList();
			return View(suppliers);
		}
	}
}
