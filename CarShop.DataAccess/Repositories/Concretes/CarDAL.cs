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

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
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
    }
}
