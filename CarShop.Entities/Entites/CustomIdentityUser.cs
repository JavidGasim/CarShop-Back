using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Entities.Entites
{
    public class CustomIdentityUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string? City { get; set; }
        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
        public virtual ICollection<Favourite> Favorites { get; set; } = new List<Favourite>();
    }
}
