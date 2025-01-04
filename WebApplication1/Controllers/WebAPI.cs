using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class WebAPI : Controller
	{
		public IActionResult PharmacyOnDuty()
		{
			string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler");
			var pharmacy = JsonConvert.DeserializeObject<List<Pharmacy>>(json);
			return View(pharmacy);
		}

		public IActionResult ArtAndCulture()
		{
			string json = new WebClient().DownloadString("https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler");

			var activity = JsonConvert.DeserializeObject<List<Activity>>(json);

			return View(activity);
		}

	}
}
