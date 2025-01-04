using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ViewComponents

{
    public class Headers: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            Baglanti context = new Baglanti();
            List<Category> categories = context.Categories.Where(c=> c.Active == true).ToList();    
            return View(categories);
        }
    }
}
