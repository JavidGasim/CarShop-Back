using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Entities.Entites
{
    public class Favourite
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public virtual CustomIdentityUser User { get; set; } = null!;
        public int CarId { get; set; }
        public virtual Car Car { get; set; } = null!;
    }
}
