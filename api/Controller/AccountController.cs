using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        
            _tokenService = tokenService;
            _userManager = userManager;
        }

        //Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if(user ==null)
            {
                return Unauthorized("Invalid username!");
            }
        var  result = await  _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);
        if(!result.Succeeded) return Unauthorized("Invalid password!");

           return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });

        }

        //Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if(roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto{
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = _tokenService.CreateToken(user)
                        });
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }

                return BadRequest(createdUser.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        
    }
}