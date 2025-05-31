using CarShop.DataAccess.Data;
using CarShop.DataAccess.Repositories.Abstracts;
using CarShop.Entities.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.DataAccess.Repositories.Concretes
{
    public class CarDAL : ICarDAL
    {
        private readonly CarShopDbContext _context;
        public CarDAL(CarShopDbContext context)
        {
            _context = context;
        }
        public async Task AddCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(Car car)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Car>> GetAllCarsAsync(int pageNumber, int pageSize)
        {
            return await _context.Cars.OrderByDescending(c => c.Id).Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars.Include(c => c.CustomIdentityUser).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Car>> GetFavCarsAsync(string userId)
        {
            return await _context.Favourites
                .Where(x => x.UserId == userId)
                .Include(x => x.Car)
                .Select(x => x.Car!)
                .ToListAsync();
        }

        public async Task AddFavCarAsync(Favourite favourite)
        {
            await _context.Favourites.AddAsync(favourite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavCarAsync(Favourite favourite)
        {
            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();
        }

        public async Task<Favourite> GetFavouriteByUserIdAndCarIdAsync(string userId, int carId)
        {
            return await _context.Favourites
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CarId == carId);
        }

        public async Task<List<Car>> GetCarsByUserIdAsync(string userId)
        {
            return await _context.Cars
                .Where(x => x.CustomIdentityUser != null && x.CustomIdentityUser.Id == userId)
                .ToListAsync();
        }
    }
}
