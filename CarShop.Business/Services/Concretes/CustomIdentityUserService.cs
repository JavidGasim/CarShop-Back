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
    public class CustomIdentityUserService : ICustomIdentityUserService
    {
        private readonly ICustomIdentityUserDAL _customIdentityUserDAL;
        public CustomIdentityUserService(ICustomIdentityUserDAL customIdentityUserDAL)
        {
            _customIdentityUserDAL = customIdentityUserDAL;
        }
        public async Task AddAsync(CustomIdentityUser user)
        {
            await _customIdentityUserDAL.AddAsync(user);
        }
        public async Task DeleteAsync(CustomIdentityUser user)
        {
            await _customIdentityUserDAL.DeleteAsync(user);
        }
        public async Task<List<CustomIdentityUser>> GetAllAsync()
        {
            return await _customIdentityUserDAL.GetAllAsync();
        }
        public async Task<CustomIdentityUser> GetByIdAsync(string id)
        {
            return await _customIdentityUserDAL.GetByIdAsync(id);
        }
        public async Task<CustomIdentityUser> GetByUserNameAsync(string name)
        {
            return await _customIdentityUserDAL.GetByUserNameAsync(name);
        }
        public async Task UpdateAsync(CustomIdentityUser user)
        {
            await _customIdentityUserDAL.UpdateAsync(user);
        }
    }
}
