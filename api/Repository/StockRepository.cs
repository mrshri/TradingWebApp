using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext _context;
        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateStockAsync(Stock stock)
        {
            await  _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
            
        }

        public async Task<Stock> DeleteStockAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if(stockModel == null)
            {
                return null;
            }   
            _context.Stocks.Remove(stockModel); 
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
          return await  _context.Stocks.ToListAsync();
        }

        public async Task<Stock> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<Stock> UpdateStockAsync(int id ,UpdateStockRequestDto stockDto)
        {
           var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
           if(existingStock==null){
            return null;
           }

             existingStock.Symbol = stockDto.Symbol;
             existingStock.CompanyName = stockDto.CompanyName;
             existingStock.Purchase = stockDto.Purchase;
             existingStock.LastDiv = stockDto.LastDiv;
             existingStock.Industry = stockDto.Industry;
             existingStock.MarketCap = stockDto.MarketCap;
                        
           await _context.SaveChangesAsync();
           return existingStock;
        }
    }
}