using AutoMapper;
using CarShop.Business.Services.Abstracts;
using CarShop.Entities.Entites;
using CarShop.WepApi.DTOS;
using CarShop.WepApi.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CarShop.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHubContext<CarHub> _hubContext;

        public CarController(ICarService carService, IMapper mapper, ICustomIdentityUserService customIdentityUserService, UserManager<CustomIdentityUser> userManager, IHubContext<CarHub> hubContext)
        {
            _carService = carService;
            _mapper = mapper;
            _customIdentityUserService = customIdentityUserService;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCar([FromBody] CarAddDto dto)
        {
            var car = _mapper.Map<Car>(dto);
            var user = await _customIdentityUserService.GetByIdAsync(dto.UserId);
            car.CustomIdentityUser = user;

            await _carService.AddCarAsync(car);

            return Ok(new { Message = "Car added successfully", car });
            //return CreatedAtAction(nameof(GetCarById), new { id = dto.Id });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCar([FromBody] CarUpdateDto dto)
        {
            var car = await _carService.GetCarByIdAsync(dto.Id);

            if (car == null)
            {
                return NotFound();
            }

            car.Marka = dto.Marka ?? car.Marka;
            car.Model = dto.Model ?? car.Model;
            car.Year = dto.Year ?? car.Year;
            car.Color = dto.Color ?? car.Color;
            car.Price = dto.Price ?? car.Price;
            car.BanType = dto.BanType ?? car.BanType;
            car.Engine = dto.Engine ?? car.Engine;
            car.March = dto.March ?? car.March;
            car.GearBox = dto.GearBox ?? car.GearBox;
            car.Gear = dto.Gear ?? car.Gear;
            car.IsNew = dto.IsNew ?? car.IsNew;
            car.Situation = dto.Situation ?? car.Situation;
            car.Description = dto.Description ?? car.Description;
            car.Url1 = dto.Url1 ?? car.Url1;
            car.Url2 = dto.Url2 ?? car.Url2;
            car.Url3 = dto.Url3 ?? car.Url3;
            if (dto.FeedBacks != null && dto.FeedBacks.Any())
            {
                car.FeedBacks ??= new List<string>(); // Əgər null-dursa initialize et
                car.FeedBacks.Add(dto.FeedBacks);

                await _hubContext.Clients.All.SendAsync("ReceiveFeedback", new
                {
                    CarId = car.Id,
                    NewFeedback = dto.FeedBacks,
                    //CarName = $"{car.Marka} {car.Model} ({car.Year})"
                });

                if (!string.IsNullOrEmpty(car.CustomIdentityUser.Id))
                {
                    await _hubContext.Clients.Group(car.CustomIdentityUser.UserName).SendAsync("NotifyOwner", new
                    {   
                        CarId = car.Id,
                        Message = $"You got new feedback for - {car.Marka} {car.Model} ({car.Year})"
                    });
                }
            }

            await _carService.UpdateCarAsync(car);

            return Ok(new { Message = "Car updated successfully", car });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            await _carService.DeleteCarAsync(car);
            return Ok(new { Message = "Car Deleted Successfully" });
        }

        [HttpGet("myFavs")]
        [Authorize]
        public async Task<IActionResult> GetMyFavs()
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var user = await _customIdentityUserService.GetByUserNameAsync(userName);

            //Console.WriteLine($"User: {user}");
            if (user == null)
            {
                return Unauthorized();
            }

            var favCars = await _carService.GetFavCarsAsync(user.Id);
            return Ok(favCars);
        }

        [HttpPost("addToFav/{carId}")]
        [Authorize]
        public async Task<IActionResult> AddToFav(int carId)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            var user = await _customIdentityUserService.GetByUserNameAsync(userName);
            if (user == null)
            {
                return Unauthorized();
            }

            var car = await _carService.GetCarByIdAsync(carId);
            if (car == null)
            {
                return NotFound();
            }
            // Assuming Favourite is a model that contains UserId and CarId
            var favourite = new Favourite
            {
                UserId = user.Id,
                CarId = carId
            };

            await _carService.AddFavCarAsync(favourite);
            return Ok(new { Message = "Car added to favourites successfully" });
        }

        [HttpDelete("removeFromFav/{carId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromFav(int carId)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _customIdentityUserService.GetByUserNameAsync(userName);
            if (user == null)
            {
                return Unauthorized();
            }
            var car = await _carService.GetCarByIdAsync(carId);
            if (car == null)
            {
                return NotFound();
            }

            var favCar = await _carService.GetFavouriteByUserIdAndCarIdAsync(user.Id, carId);
            // Assuming Favourite is a model that contains UserId and CarId
            //var favourite = new Favourite
            //{
            //    UserId = user.Id,
            //    CarId = carId
            //};
            await _carService.RemoveFavCarAsync(favCar);
            return Ok(new { Message = "Car removed from favourites successfully" });
        }

        [HttpGet("getUserCar/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserCar(string userId)
        {
            var user = await _customIdentityUserService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            var cars = await _carService.GetCarsByUserIdAsync(userId);
            return Ok(cars);
        }
    }
}