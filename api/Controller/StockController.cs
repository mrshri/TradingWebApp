using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [ApiController]
    [Route("api/Stock")]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StockController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult GetAllStocks(){
         var stocks = _context.Stocks.ToList().Select
         (s=>s.ToStockDto());
         if (stocks == null || !stocks.Any())
         {
            return NotFound("No stocks found.");
         }
         return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetStockById([FromRoute]int id){
            var stock = _context.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }
            return Ok(stock.ToStockDto()); 
        }

        [HttpPost]
        public IActionResult CreateStock([FromBody] CreateStockRequestDto stockDto){
            var stockModel = stockDto.ToStockFromCreateDto();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto){
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }

             stock.Symbol = stockDto.Symbol;
             stock.CompanyName = stockDto.CompanyName;
             stock.Purchase = stockDto.Purchase;
             stock.LastDiv = stockDto.LastDiv;
             stock.Industry = stockDto.Industry;
             stock.MarketCap = stockDto.MarketCap;
             _context.Stocks.Update(stock);           
             _context.SaveChanges();
             
             return Ok(stock.ToStockDto());
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStock([FromRoute] int id){
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }
            _context.Stocks.Remove(stock);
            _context.SaveChanges();
            return NoContent();
        }
        
    }
}