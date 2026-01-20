using ecommerce_csharp.Models;

namespace ecommerce_csharp.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Prevent duplicate seeding
            if (context.Sneakers.Any())
                return;

            context.Sneakers.AddRange(
                new Sneaker
                {
                    Name = "Air Force 1",
                    Brand = "Nike",
                    Price = 2599,
                    Gender = "Women",
                    StockQuantity = 10,
                    ImageUrl = "/images/AirMaxPlus_W.webp"
                },
                new Sneaker
                {
                    Name = "Vapor",
                    Brand = "Nike",
                    Price = 2499,
                    Gender = "Women",
                    StockQuantity = 5,
                    ImageUrl = "/images/P6000.webp"
                }
            );

            context.SaveChanges();
        }
    }
}

