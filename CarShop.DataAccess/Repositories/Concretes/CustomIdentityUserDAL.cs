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
    public class CustomIdentityUserDAL : ICustomIdentityUserDAL
    {
        private readonly CarShopDbContext _context;

        public CustomIdentityUserDAL(CarShopDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CustomIdentityUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(CustomIdentityUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<CustomIdentityUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<CustomIdentityUser> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<CustomIdentityUser> GetByUserNameAsync(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == name);
        }
        public async Task UpdateAsync(CustomIdentityUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
