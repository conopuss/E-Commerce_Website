using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ViewComponents
{
	public class Address:ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			Baglanti context = new Baglanti();
			var settings = context.Settings.FirstOrDefault(p => p.SettingID == 1);
		
			
			
			return View(settings);
		}

	}
}
