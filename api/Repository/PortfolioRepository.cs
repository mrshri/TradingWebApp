using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
  
    public class PortfolioRepository : IPortfolioRepository
    {
          private readonly AppDbContext _context;
         public PortfolioRepository(AppDbContext context)
        {
             _context = context;
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var userPortfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == appUser.Id
            && p.Stock.Symbol == symbol.ToLower());
            if (userPortfolio == null)
            {
                return null;
            }
            _context.Portfolios.Remove(userPortfolio);
            await _context.SaveChangesAsync();
            return userPortfolio;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios
                .Where(p => p.AppUserId == user.Id)
                .Select(p => new Stock{
                    Id = p.StockId,
                    Symbol = p.Stock.Symbol,
                    CompanyName = p.Stock.CompanyName,
                    Purchase = p.Stock.Purchase,
                    LastDiv = p.Stock.LastDiv,
                    Industry = p.Stock.Industry,
                    MarketCap = p.Stock.MarketCap,
                    
                }).ToListAsync();
                
        }
    }
}