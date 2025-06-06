﻿using AutoMapper;
using CarShop.Business.Services.Abstracts;
using CarShop.Entities.Entites;
using CarShop.WepApi.DTOS;
using CarShop.WepApi.Services.Abstracts;
using CarShop.WepApi.Services.Concretes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarShop.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly RoleManager<CustomIdentityRole> _roleManager;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;


        public AccountController(UserManager<CustomIdentityUser> userManager, IMapper mapper, SignInManager<CustomIdentityUser> signInManager, RoleManager<CustomIdentityRole> roleManager, IConfiguration configuration, ICustomIdentityUserService customIdentityUserService, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _customIdentityUserService = customIdentityUserService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _emailService = emailService;
        }

        [HttpPost("existUser")]
        public async Task<IActionResult> ExistUser([FromQuery] string name)
        {
            var existingUser = await _userManager.FindByNameAsync(name);
            if (existingUser != null)
            {
                return BadRequest(new { Status = "Name Error", Message = "A user with this username already exists!" });
            }

            return Ok(new { Status = "Success" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {

            var user = new CustomIdentityUser
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                ProfilePicture = dto.ImagePath,
                City = dto.City,
                PhoneNumber = dto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(dto.Role))
                {
                    await _roleManager.CreateAsync(new CustomIdentityRole { Name = dto.Role });
                }

                await _userManager.AddToRoleAsync(user, dto.Role);

                return Ok(new { Status = "Success", Message = "User created successfuly!" });
            }

            return BadRequest(new { Status = "Error", Message = "User creation failed!", Error = result.Errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(dto.UserName);

                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    };

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = GetToken(authClaims);

                var identity = new ClaimsIdentity(authClaims, "Registration");
                var principal = new ClaimsPrincipal(identity);

                _httpContextAccessor.HttpContext!.User = principal;

                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo, Role = userRoles.FirstOrDefault() });
            }

            return Unauthorized();
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        [HttpPost("send-code")]
        public IActionResult SendVerificationCode([FromQuery] string email)
        {
            try
            {
                var code = _emailService.SendVerificationCode(email);
                return Ok(new { Message = "Verification code sent successfully.", Code = code });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error sending verification code.", Error = ex.Message });
            }
        }

        [HttpPost("verify-code")]
        public IActionResult CheckVerificationCode([FromQuery] int code)
        {
            var result = _emailService.CheckVerificationCode(code);
            if (result.IsValid)
            {
                return Ok(new { Message = result.Message });
            }

            return BadRequest(new { Message = result.Message });
        }
        [Authorize]
        [HttpGet("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var currentUser = await _customIdentityUserService.GetByUserNameAsync(userName);

            if (currentUser == null)
            {
                return NotFound(new { Message = "Current user not found" });
            }

            //var userRole = await _userManager.GetRolesAsync(currentUser);
            var currentUserDto = _mapper.Map<UserDto>(currentUser);
            return Ok(new { User = currentUserDto });
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto dto)
        {
            // 1. Mövcud istifadəçini tapırıq
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var user = await _customIdentityUserService.GetByUserNameAsync(userName);

            if (user == null)
                return NotFound(new { message = "İstifadəçi tapılmadı." });

            // 2. Gələn məlumatları tətbiq edirik
            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.Email = dto.Email ?? user.Email;
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
            user.City = dto.City ?? user.City;
            user.ProfilePicture = dto.ProfilePicture ?? user.ProfilePicture;

            // 3. Məlumatı yeniləyirik
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new { message = "Yeniləmə alınmadı.", errors = result.Errors });

            return Ok(new { message = "Profil uğurla yeniləndi." });
        }

        [Authorize]
        [HttpGet("getUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _customIdentityUserService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }
    }
}
