using CarShop.WepApi.DTOS;

namespace CarShop.WepApi.Services.Abstracts
{
    public interface IPhotoService
    {
        Task<string> UploadImageAsync(PhotoCreationDto dto);

    }
}
