using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetUserPortfolios()
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            if (userPortfolio == null)
            {
                return NotFound("Portfolio not found");
            }

            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var stock = await _stockRepository.GetStockBySymbolAsync(symbol);
            if (stock == null)
            {
                return NotFound("Stock not found");
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            if (userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Stock already exists in portfolio");
            }

            var portfolio = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await _portfolioRepository.CreateAsync(portfolio);
            if (portfolio == null)
            {
                return StatusCode(500, "Error adding stock to portfolio");
            }
            else
            {
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var stock = await _stockRepository.GetStockBySymbolAsync(symbol);
            if (stock == null)
            {
                return NotFound("Stock not found");
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower());
            if (filteredStock.Count() == 1)
            {
                await _portfolioRepository.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not found in portfolio");
            }  

          return Ok("Stock removed from portfolio");
            
        }
    }
}