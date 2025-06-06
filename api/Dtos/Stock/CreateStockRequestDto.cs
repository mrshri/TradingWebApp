using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol must be at most 10 characters long.")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(20, ErrorMessage = "Company name must be at most 20 characters long.")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1,10000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001,100)]
        public decimal LastDiv { get; set; }
        [Required]
         [MaxLength(20, ErrorMessage = "Industry must be at most 20 characters long.")]
        public string Industry { get; set; }= string.Empty;
        [Range(1,500000000)]
        public long MarketCap { get; set; }
    }
}