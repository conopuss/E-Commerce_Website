using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.ViewComponents
{
    public class Footers:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
