using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller
{
    [ApiController]
    [Route("api/Stock")]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStockRepository _stockRepository;
        public StockController(AppDbContext context, IStockRepository stockRepository)
        {
            _context = context;
            _stockRepository = stockRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllStocks([FromQuery] QueryObject query){
            
         var stocks = await _stockRepository.GetAllStocksAsync(query);

         var stockDto = stocks.Select(s=>s.ToStockDto());
         if (stocks == null || !stocks.Any())
         {
            return NotFound("No stocks found.");
         }
         return Ok(stocks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockById([FromRoute]int id){
            var stock = await _stockRepository.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }
            return Ok(stock.ToStockDto()); 
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();

            await _stockRepository.CreateStockAsync(stockModel);
            return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id:int}")]
        [Authorize]        
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            var stock = await _stockRepository.UpdateStockAsync(id, stockDto);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }


            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var deletedStock = await _stockRepository.DeleteStockAsync(id);
            if (deletedStock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }
            return NoContent();
        }
        
    }
}