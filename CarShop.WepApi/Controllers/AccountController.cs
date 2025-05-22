using AutoMapper;
using CarShop.Business.Services.Abstracts;
using CarShop.Entities.Entites;
using CarShop.WepApi.DTOS;
using CarShop.WepApi.Services.Abstracts;
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
        private readonly IEmailSender _emailSender;


        public AccountController(UserManager<CustomIdentityUser> userManager, IMapper mapper, SignInManager<CustomIdentityUser> signInManager, RoleManager<CustomIdentityRole> roleManager, IConfiguration configuration, ICustomIdentityUserService customIdentityUserService, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _customIdentityUserService = customIdentityUserService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _emailSender = emailSender;
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
        public async Task<IActionResult> SendVerificationCode([FromBody] EmailDto dto)
        {
            var code = new Random().Next(100000, 999999).ToString();
            var message = string.Format(@"
  <html>
    <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
      <div style='background-color: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
        <h2 style='color: #333;'>Salam!</h2>
        <p style='color: #555;'>Sizin təsdiq kodunuz aşağıdadır:</p>
        <div style='font-size: 24px; color: #2c3e50; font-weight: bold; margin: 10px 0;'>{0}</div>
        <p style='color: #777;'>Zəhmət olmasa bu kodu 5 dəqiqə ərzində istifadə edin.</p>
        <hr />
        <p style='font-size: 12px; color: #aaa;'>Bu mesaj avtomatik göndərilmişdir.</p>
      </div>
    </body>
  </html>
", code);

            // kodu yadda saxlayırıq (DB, Cache və ya Memory)
            // Məsələn: TempData, Session, ya Redis ilə saxlanıla bilər
            HttpContext.Session.SetString(dto.Email, code);

            await _emailSender.SendEmailAsync(dto.Email, "Email Verification", message);

            return Ok(new { Message = "Verification code sent!" });
        }

        [HttpPost("verify-code")]
        public IActionResult VerifyCode([FromBody] CodeVerificationDto dto)
        {
            var savedCode = HttpContext.Session.GetString(dto.Email);

            if (savedCode == dto.Code)
            {
                // İstifadəçini qeydiyyatdan keçirə bilərsiniz
                return Ok(new { Message = "Verification successful" });
            }

            return BadRequest(new { Message = "Invalid verification code" });
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
    }
}
