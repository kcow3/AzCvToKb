using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FormsToKeyboard
{
    public static class ImageHelper
    {
        /// <summary>
        /// Converts the image to a string with the PNG format
        /// The image must be presented in JPEG, PNG, GIF, or BMP format
        /// https://docs.microsoft.com/en-gb/azure/cognitive-services/computer-vision/overview
        /// </summary>
        /// <param name="image">Image to convert</param>
        /// <returns>stream of the image</returns>
        public static Stream ToStream(this Image image)
        {
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// For the Read API, the dimensions of the image must be between 50 x 50 and 10000 x 10000 pixels
        /// https://docs.microsoft.com/en-gb/azure/cognitive-services/computer-vision/overview
        /// </summary>
        /// <param name="image">image to validate</param>
        /// <returns>boolean based on validation result</returns>
        public static bool IsDimentionsOk(this Image image)
        {
            try
            {
                if (image == null) return false;

                if (image.Width <= 50 || image.Width >= 10000) return false;

                if (image.Height <= 50 || image.Height >= 10000) return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
