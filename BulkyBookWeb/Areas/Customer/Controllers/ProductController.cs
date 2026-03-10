using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using BulkyBookWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService= productService;
        }
        public async Task<IActionResult> Index()
        {
           
            return View();
        }

        public async Task<IActionResult> Upsert()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Upsert")]
        public async Task<IActionResult> UpsertPOST(Product product)
        {

            if (ModelState.IsValid)
            {
                await _productService.CreateProductAsync(product);
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int id)
        {
            await _productService.DeleteProductAsync(id);
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
            
        }



        #region API CALLS
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync(true);
            return Json(new { data = products });
        }
        #endregion
    }
}
