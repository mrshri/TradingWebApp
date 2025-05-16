using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
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

        public async Task<Stock?> DeleteStockAsync(int id)
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

        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {
          var stocks =  _context.Stocks.Include(c=>c.Comments).AsQueryable();
          //filtering stocks
          if(!string.IsNullOrWhiteSpace(query.CompanyName))
          {stocks = stocks.Where(s => s.CompanyName.ToLower().Contains(query.CompanyName));
          }
          if(!string.IsNullOrWhiteSpace(query.Symbol))
          {
            stocks = stocks.Where(s =>s.Symbol.Contains(query.Symbol));
          }
           //sorting
            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
               if(query.SortBy.Equals("Symbol",StringComparison.OrdinalIgnoreCase))
               {
                stocks = query.IsDescending ? stocks.OrderByDescending(s =>s.Symbol) : stocks.OrderBy(s => s.Symbol) ;
               }
            }

            //pagination
            var skipPages = (query.PageNumber-1) * query.PageNumber;
            
            return await stocks.Skip(skipPages).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.Include(c=>c.Comments).FirstOrDefaultAsync(i=>i.Id==id);;
        }

        public Task<Stock?> GetStockBySymbolAsync(string symbol)
        {
            return _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<bool> IsStockExists(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);

        }

        public async Task<Stock?> UpdateStockAsync(int id ,UpdateStockRequestDto stockDto)
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