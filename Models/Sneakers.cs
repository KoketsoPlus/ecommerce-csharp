using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce_csharp.Models
{
        public class Sneaker
        {
            public int Id { get; set; }

            [Required]
            public required string Name { get; set; }

            [Required]
            public required string Brand { get; set; }

            [Column(TypeName = "decimal(18,2)")]
            [Range(0.01, 100000)]
            public decimal Price { get; set; }

            public int StockQuantity { get; set; }

            public string? ImageUrl { get; set; }
            public required string Gender { get; set; }
        }
}
