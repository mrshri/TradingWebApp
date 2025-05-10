using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetAllStocks(){
            
         var stocks = await _context.Stocks.ToListAsync();

         var stockDto = stocks.Select(s=>s.ToStockDto());
         if (stocks == null || !stocks.Any())
         {
            return NotFound("No stocks found.");
         }
         return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById([FromRoute]int id){
            var stock =await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }
            return Ok(stock.ToStockDto()); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto){
            var stockModel = stockDto.ToStockFromCreateDto();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto){
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
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
            //  _context.Stocks.Update(stock);           
             await _context.SaveChangesAsync();
             
             return Ok(stock.ToStockDto());
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id){
            var stock =await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }
           _context.Stocks.Remove(stock);
          await  _context.SaveChangesAsync();
            return NoContent();
        }
        
    }
}