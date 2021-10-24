using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace ChatTogether.Commons.ImageService
{
    public interface IImageService
    {
        Bitmap CreateThumbnail(IFormFile file);
    }
}
