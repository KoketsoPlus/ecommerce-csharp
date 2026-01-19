using ecommerce_csharp.Data;
using ecommerce_csharp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_csharp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;

        public CartController(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // POST: Cart/Add/5
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Add(int id)
        {
            var sneaker = await _context.Sneakers.FindAsync(id);

            if (sneaker == null)
            {
                return NotFound();
            }

            _cartService.AddToCart(sneaker);

            return RedirectToAction("Index", "Sneakers");
        }
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            ViewBag.Total = _cartService.GetTotal();
            return View(cart);
        }
        [HttpPost]
        public IActionResult Remove(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
