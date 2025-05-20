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
        public virtual IEnumerable<Car>? Cars { get; set; }
        public CustomIdentityUser()
        {
            Cars = new List<Car>();
        }

    }
}
