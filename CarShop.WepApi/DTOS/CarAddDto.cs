using System.ComponentModel.DataAnnotations;

namespace CarShop.WepApi.DTOS
{
    public class CarAddDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Color { get; set; }
        [Required]

        public string? Url1 { get; set; }
        [Required]
        public string? Url2 { get; set; }
        [Required]
        public string? Url3 { get; set; }
        [Required]
        public string? Price { get; set; }
        [Required]
        public string? Marka { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        public string? Year { get; set; }
        [Required]
        public string? BanType { get; set; }
        [Required]

        public string? Engine { get; set; }
        [Required]
        public string? March { get; set; }
        [Required]
        public string? GearBox { get; set; }
        [Required]
        public string? Gear { get; set; }
        [Required]
        public string? IsNew { get; set; }
        [Required]
        public string? Situation { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public string? FuelType { get; set; }
        [Required]
        public string? City { get; set; }

    }
}
