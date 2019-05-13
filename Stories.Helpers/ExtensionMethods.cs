using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Net.Mime;
using System.Drawing;

namespace Stories.Helpers
{

    public static class ExtensionMethods
    {
        public static byte[] ReduceSize(this byte[] data, double w = 400, double h = 400)
        {
            using (var ms = new MemoryStream(data))
            {
                var image = Image.FromStream(ms);

                var ratioX = w / image.Width;
                var ratioY = h / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var width = (int)(image.Width * ratio);
                var height = (int)(image.Height * ratio);

                var newImage = new Bitmap(width, height);
                Graphics.FromImage(newImage).DrawImage(image, 0, 0, width, height);
                Bitmap bmp = new Bitmap(newImage);

                ImageConverter converter = new ImageConverter();

                data = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                return data;
            }
        }
    }
}
