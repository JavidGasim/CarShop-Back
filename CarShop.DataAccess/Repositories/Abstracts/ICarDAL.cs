using CarShop.Entities.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.DataAccess.Repositories.Abstracts
{
    public interface ICarDAL
    {
        Task<List<Car>> GetAllCarsAsync(int pageNumber, int pageSize);
        Task<Car> GetCarByIdAsync(int id);
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(Car car);
        Task<List<Car>> GetFavCarsAsync(string userId);
        Task AddFavCarAsync(Favourite favourite);
        Task RemoveFavCarAsync(Favourite favourite);
        Task<Favourite> GetFavouriteByUserIdAndCarIdAsync(string userId, int carId);
        Task<List<Car>> GetCarsByUserIdAsync(string userId);
    }
}
