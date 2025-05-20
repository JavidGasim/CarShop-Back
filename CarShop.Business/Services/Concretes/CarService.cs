using CarShop.Business.Services.Abstracts;
using CarShop.DataAccess.Repositories.Abstracts;
using CarShop.Entities.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Business.Services.Concretes
{
    public class CarService : ICarService
    {
        private readonly ICarDAL _carDAL;

        public CarService(ICarDAL carDAL)
        {
            _carDAL = carDAL;
        }

        public async Task AddCarAsync(Car car)
        {
            await _carDAL.AddCarAsync(car);
        }

        public async Task DeleteCarAsync(Car car)
        {
            await _carDAL.DeleteCarAsync(car);
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _carDAL.GetAllCarsAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _carDAL.GetCarByIdAsync(id);
        }

        public async Task UpdateCarAsync(Car car)
        {
            await _carDAL.UpdateCarAsync(car);
        }

        public async Task<List<Car>> GetFavCarsAsync(string userId)
        {
            return await _carDAL.GetFavCarsAsync(userId);
        }
    }
}
