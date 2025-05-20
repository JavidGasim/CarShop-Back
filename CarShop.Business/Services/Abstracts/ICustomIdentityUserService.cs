using CarShop.Entities.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Business.Services.Abstracts
{
    public interface ICustomIdentityUserService
    {
        Task<List<CustomIdentityUser>> GetAllAsync();
        Task<CustomIdentityUser> GetByIdAsync(string id);
        Task<CustomIdentityUser> GetByUserNameAsync(string name);
        Task AddAsync(CustomIdentityUser user);
        Task UpdateAsync(CustomIdentityUser user);
        Task DeleteAsync(CustomIdentityUser user);
    }
}
