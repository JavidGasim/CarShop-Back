using CarShop.WepApi.DTOS;
using CarShop.WepApi.Services.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarShop.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public ImageController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("newImage")]
        public async Task<IActionResult> Post()
        {
            var file = Request.Form.Files.GetFile("file");

            if (file != null && file.Length > 0)
            {
                string result = await _photoService.UploadImageAsync(new PhotoCreationDto { File = file });

                return Ok(new { ImageUrl = result });
            }

            return BadRequest(new { Message = "Photo Creation Failed!" });
        }
    }
}
