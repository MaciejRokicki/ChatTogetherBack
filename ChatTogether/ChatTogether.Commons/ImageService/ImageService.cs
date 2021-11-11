 using ChatTogether.Commons.ConfigurationModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
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
            Size sourceImageSize = sourceImage.Size;

            double imageRatioX = (double)imageConfiguration.MaxThumbnailWidth / sourceImageSize.Width;
            double imageRatioY = (double)imageConfiguration.MaxThumbnailHeight / sourceImageSize.Height;
            double imageRatio = Math.Min(imageRatioX, imageRatioY);

            int thumbnailWidth = (int)(sourceImageSize.Width * imageRatio);
            int thumbnailHeight = (int)(sourceImageSize.Height * imageRatio);

            Bitmap thumbnail = new Bitmap(sourceImage, new Size(thumbnailWidth, thumbnailHeight));

            return thumbnail;
        }
    }
}
