using CarShop.Entities.Entites;

namespace CarShop.WepApi.DTOS
{
    public class CarUpdateDto
    {
        public int Id { get; set; }
        public string? Color { get; set; }
        public string? Url1 { get; set; }
        public string? Url2 { get; set; }
        public string? Url3 { get; set; }
        public string? Price { get; set; }
        public string? Marka { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public string? BanType { get; set; }
        public string? Engine { get; set; }
        public string? March { get; set; }
        public string? GearBox { get; set; }
        public string? Gear { get; set; }
        public string? IsNew { get; set; }
        public string? Situation { get; set; }
        public string? Description { get; set; }
        //public virtual CustomIdentityUser? CustomIdentityUser { get; set; }
        //public virtual ICollection<Favourite> Favorites { get; set; } = new List<Favourite>();
    }
}
