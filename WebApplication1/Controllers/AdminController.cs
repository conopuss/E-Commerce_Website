using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        Baglanti context = new Baglanti();

        Cls_Category cls_Category = new Cls_Category();
        Cls_User cls_User = new Cls_User();
        Cls_Supplier cls_Supplier = new Cls_Supplier();
        Cls_Status cls_Status = new Cls_Status();
        Cls_Product cls_Product = new Cls_Product();
        Cls_Setting cls_Setting = new Cls_Setting();


        

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View();
            }
            else
            {
                TempData["error"] = "Admin girişi yapınız";
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult CategoryIndex()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                List<Category> categories = cls_Category.CategorySelect("all");
                return View(categories); // cshtml sayfasında modele verecek (kategori listesi)
            }
            else
            {
                TempData["error"] = "Admin girişi yapınız";
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {

            CategoryFill("main");
            return View();

        }

        public IActionResult CategoryCreate(Category category)
        {
            if (ModelState.IsValid)
            {
                bool answer = Cls_Category.CategoryInsert(category);
                if (answer)
                {
                    TempData["Message"] = category.CategoryName + " kategorisi eklendi";
                }
                else
                {
                    TempData["Message"] = "HATA";
                }

            }

            return RedirectToAction("CategoryCreate");
            //return RedirectToAction(nameof(CategoryCreate));   Bu şekilde de yazılabilir

        }

        void CategoryFill(string main_or_all)
        {
            List<Category> categories = cls_Category.CategorySelect(main_or_all);
            ViewData["categoryList"] = categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryID.ToString()
            });
        }


        public async Task<ActionResult> CategoryEdit(int? id)
        {
            CategoryFill("main");
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }

            var category = await cls_Category.CategoryDetails(id);

            return View(category);
        }

        [HttpPost]
        public IActionResult CategoryEdit(Category category)
        {
            bool answer = Cls_Category.CategoryUpdate(category);
            if (answer)
            {
                TempData["Message"] = category.CategoryName + " Kategorisi güncellendi";
                return RedirectToAction("CategoryIndex");
            }
            else
            {
                TempData["Message"] = "HATA!";
                return RedirectToAction(nameof(CategoryEdit));
            }
        }

        public async Task<IActionResult> CategoryDetails(int? id)
        {
            var category = await cls_Category.CategoryDetails(id);
            ViewBag.ProductCount = context.Products.Count(p => p.CategoryID == id);
            return View(category);
        }


        [HttpGet]

        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null || context.Categories == null)
            {
                return NotFound();
            }
            var category = await cls_Category.CategoryDetails(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult CategoryDelete(int id)
        {
            bool answer = Cls_Category.CategoryDelete(id);
            if (answer)
            {
                TempData["success"] = "Silindi";
                return RedirectToAction(nameof(CategoryIndex));
            }
            else
            {
                TempData["error"] = "HATA!";
                return RedirectToAction(nameof(CategoryDelete));
            }
        }

        public IActionResult SupplierIndex()
        {
            //session için program.cs nin içine satır ekledik
            if (HttpContext.Session.GetString("Admin") != null)
            {
                List<Supplier> suppliers = cls_Supplier.SupplierSelect();
                return View(suppliers); // cshtml sayfasında modele verecek (marka listesi)
            }
            else
            {
                TempData["error"] = "Admin girişi yapınız";
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public IActionResult SupplierCreate()
        {

            return View();
        }

        [HttpPost]
        public IActionResult SupplierCreate(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                bool answer = Cls_Supplier.SupplierInsert(supplier);
                if (answer)
                {
                    TempData["success"] = supplier.BrandName + " markası eklendi";
                    return RedirectToAction(nameof(SupplierIndex));
                }
                else
                {
                    TempData["error"] = "HATA!";
                }
            }
            return RedirectToAction(nameof(SupplierCreate));
        }
        [HttpGet]
        public IActionResult SupplierEdit(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();

            }
            var supplier = cls_Supplier.SupplierDetails(id);
            return View(supplier);
        }
        [HttpPost]
        public IActionResult SupplierEdit(Supplier supplier)
        {
            if (supplier.PhotoPath == null)
            {
                supplier.PhotoPath = context.Suppliers.FirstOrDefault(s => s.SupplierID == supplier.SupplierID).PhotoPath.ToString();
            }


            bool answer = Cls_Supplier.SupplierUpdate(supplier);
            if (answer)
            {
                TempData["success"] = supplier.BrandName + " markası güncellendi";
                return RedirectToAction(nameof(SupplierIndex));
            }
            else
            {
                TempData["error"] = "HATA!";
                return RedirectToAction(nameof(SupplierEdit));
            }
        }
        public IActionResult SupplierDetails(int? id)
        {
            var supplier = cls_Supplier.SupplierDetails(id);
            ViewBag.ProductCount = context.Products.Where(c => c.SupplierID == id).Count();
            return View(supplier);
        }
        [HttpGet]
        public IActionResult SupplierDelete(int? id)
        {
            if (id == null || context.Suppliers == null)
            {
                return NotFound();

            }
            var supplier = cls_Supplier.SupplierDetails(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public IActionResult SupplierDelete(int id)
        {
            bool answer = cls_Supplier.SupplierDelete(id);
            if (answer)
            {
                TempData["success"] = "Silindi";
                return RedirectToAction(nameof(SupplierIndex));
            }
            else
            {
                TempData["error"] = "HATA!";
                return RedirectToAction(nameof(SupplierDelete), new { status = "1" });
            }
        }

        public IActionResult StatusIndex()
        {

            if (HttpContext.Session.GetString("Admin") != null)
            {
                List<Status> status = cls_Status.StatusSelect();
                return View(status);
            }

            else
            {
                TempData["error"] = "Admin girişi yapınız";
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public IActionResult StatusCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StatusCreate(Status status)
        {
            if (ModelState.IsValid)
            {
                bool answer = Cls_Status.StatusCreate(status);
                if (answer)
                {
                    TempData["success"] = status.StatusName + " statüsü eklendi";
                    return RedirectToAction(nameof(StatusIndex));
                }
                else
                {
                    TempData["error"] = "HATA!";
                    return RedirectToAction(nameof(StatusCreate));
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult StatusEdit(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();
            }
            var status = cls_Status.StatusDetails(id);
            return View(status);
        }

        [HttpPost]
        public IActionResult StatusEdit(Status status)
        {
            if (ModelState.IsValid)
            {
                bool answer = Cls_Status.StatusUpdate(status);
                if (answer)
                {
                    TempData["success"] = status.StatusName + " statüsü güncellendi";
                    return RedirectToAction(nameof(StatusIndex));
                }
                else
                {
                    TempData["error"] = "HATA!";
                    return RedirectToAction(nameof(StatusEdit));
                }
            }
            return View();
        }
        public IActionResult StatusDetails(int id)
        {
            var status = cls_Status.StatusDetails(id);
            return View(status);
        }
        [HttpGet]
        public IActionResult StatusDelete(int? id)
        {
            if (id == null || context.Statuses == null)
            {
                return NotFound();

            }
            var statuses = cls_Status.StatusDetails(id);
            if (statuses == null)
            {
                return NotFound();
            }
            return View(statuses);
        }

        [HttpPost]
        public IActionResult StatusDelete(int id)
        {
            bool answer = Cls_Status.StatusDelete(id);
            if (answer)
            {
                TempData["success"] = "Silindi";
                return RedirectToAction(nameof(StatusIndex));
            }
            else
            {
                TempData["error"] = "HATA!";
                return RedirectToAction(nameof(StatusDelete), new { status = "1" });
            }
        }
        public IActionResult ProductIndex()
        {
            //session için program.cs nin içine satır ekledik
            if (HttpContext.Session.GetString("Admin") != null)
            {
                List<Product> products = cls_Product.ProductSelect("", 0);
                return View(products);
            }
            else
            {
                TempData["error"] = "Admin girişi yapınız";
                return RedirectToAction("Login", "Home");
            }

        }
        public IActionResult ProductCreate()
        {
            CategoryFill("all");
            SupplierFill();
            StatusFill();
            return View();
        }

        void SupplierFill()
        {
            List<Supplier> suppliers = cls_Supplier.SupplierSelect();
            ViewData["supplierList"] = suppliers.Select(c => new SelectListItem { Text = c.BrandName, Value = c.SupplierID.ToString() });
        }

        void StatusFill()
        {
            List<Status> statuses = cls_Status.StatusSelect();
            ViewData["statusList"] = statuses.Select(c => new SelectListItem { Text = c.StatusName, Value = c.StatusID.ToString() });
        }

        [HttpPost]
        public IActionResult ProductCreate(Product product)
        {
            if (ModelState.IsValid)
            {
                bool answer = Cls_Product.ProductInsert(product);
                if (answer)
                {
                    TempData["success"] = product.ProductName + " ürünü eklendi";

                }
                else
                {
                    TempData["error"] = "HATA!";
                    return RedirectToAction(nameof(ProductCreate));
                }

            }
            return RedirectToAction(nameof(ProductIndex));
        }
        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            CategoryFill("all");
            StatusFill();
            SupplierFill();
            if (id == null || context.Products == null)
            {
                return NotFound();
            }
            var products = cls_Product.ProductDetails(id);
            return View(products);
        }

        [HttpPost]
        public IActionResult ProductEdit(Product product)
        {
            Product? prd = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
            product.AddDate = prd.AddDate;
            product.HighLighted = prd.HighLighted;
            product.TopSeller = prd.TopSeller;
            if (product.PhotoPath == null)
            {
                string? PhotoPath = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID).PhotoPath;
                product.PhotoPath = PhotoPath;
            }
            if (ModelState.IsValid)
            {
                bool answer = Cls_Product.ProductUpdate(product);
                if (answer)
                {
                    TempData["success"] = product.ProductName + " ürünü güncellendi";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = "HATA!";
                    return RedirectToAction(nameof(ProductEdit));
                }

            }
            return View(product);
        }
        public IActionResult ProductDetails(int? id)
        {
            var product = cls_Product.ProductDetails(id);
            return View(product);
        }

        [HttpGet]
        public ActionResult ProductDelete(int? id)
        {
            if (id == null || context.Products == null)
            {
                return NotFound();
            }

            var product = cls_Product.ProductDetails(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }

        [HttpPost]
        public IActionResult ProductDelete(int id)
        {
            if (ModelState.IsValid)
            {
                bool answer = Cls_Product.ProductDelete(id);
                if (answer)
                {
                    TempData["success"] = "Silindi";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = "HATA!";
                    return RedirectToAction(nameof(ProductDelete));

                }
            }
            return View();
        }

        public IActionResult SettingEdit()
        {
            //session için program.cs nin içine satır ekledik
            if (HttpContext.Session.GetString("Admin") != null)
            {
                var setting = cls_Setting.SettingDetail();
                return View(setting);
            }
            else
            {
                TempData["error"] = "Admin girişi yapınız";
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public IActionResult SettingEdit(Setting setting)
        {

            if (ModelState.IsValid)
            {
                bool answer = Cls_Setting.SettingUpdate(setting);
                if (answer)
                {
                    TempData["success"] = "Güncellendi";

                }
                else
                {
                    TempData["error"] = "HATA!";
                }

            }
            return RedirectToAction(nameof(SettingEdit));

        }




















    }
}
