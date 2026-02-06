using Microsoft.AspNetCore.Mvc;
using lab6remake.Models;
using lab6remake.Services;

namespace lab6remake.Controllers.Web
{
    public class ProductsController : Controller
    {
        private readonly IProductApiService _productService;

        public ProductsController(IProductApiService productService)
        {
            _productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductAsync(product);
                if (result != null)
                {
                    TempData["Success"] = "Thêm sản phẩm thành công";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Có lỗi xảy ra khi thêm sản phẩm";
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProductAsync(id, product);
                if (result)
                {
                    TempData["Success"] = "Cập nhật sản phẩm thành công";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật sản phẩm";
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result)
            {
                TempData["Success"] = "Xóa sản phẩm thành công";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi xóa sản phẩm";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}