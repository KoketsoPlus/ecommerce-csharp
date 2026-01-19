using ecommerce_csharp.Models;
using Microsoft.AspNetCore.Http; // Cart Functions
using System.Text.Json;   //Very important for cart functioning


namespace ecommerce_csharp.Services
{
    public class CartService   //Calling the cart model
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Get cart from session
        public List<CartItem> GetCart()
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            var cartJson = session.GetString(CartKey);

            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>(); ;
        }

        // Save cart to session
        private void SaveCart(List<CartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }

        // Add item to cart
        public void AddToCart(Sneaker sneaker, int quantity = 1)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.Sneaker.Id == sneaker.Id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    Sneaker = sneaker,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }

            SaveCart(cart);
        }

        // Remove item
        public void RemoveFromCart(int sneakerId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.Sneaker.Id == sneakerId);

            if (item == null)
                return;

            if (item.Quantity > 1)
            {
                item.Quantity--;
            }
            else
            {
                cart.Remove(item);
            }
            SaveCart(cart);
        }

        // Cart total
        public decimal GetTotal()
        {
            return GetCart().Sum(i => i.Sneaker.Price * i.Quantity);
        }

        // Clear cart
        public void ClearCart()
        {
            SaveCart(new List<CartItem>());
        }
    }
}
