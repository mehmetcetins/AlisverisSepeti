using ImageMagick;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Utils
{
    public static class ImageUtils
    {
        public static Bitmap ResizeImage(Stream stream,int width,int height)
        {

            MagickImage imageMagick = new MagickImage (stream);
            imageMagick.Resize(width, height);

            return imageMagick.ToBitmap();
        }
    }
}
