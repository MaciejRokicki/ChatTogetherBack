using ChatTogether.Commons.ConfigurationModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace ChatTogether.Commons.ImageService
{
    public class ImageService : IImageService
    {
        private readonly ImageConfiguration imageConfiguration;

        public ImageService(IOptions<ImageConfiguration> imageConfiguration)
        {
            this.imageConfiguration = imageConfiguration.Value;
        }

        public Bitmap CreateThumbnail(IFormFile file)
        {
            Image sourceImage = Image.FromStream(file.OpenReadStream());
            Bitmap thumbnail = new Bitmap(sourceImage, new Size(imageConfiguration.MaxThumbnailWidth, imageConfiguration.MaxThumbnailHeight));

            return thumbnail;
        }
    }
}
