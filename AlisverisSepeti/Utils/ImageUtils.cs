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
        private static string imageRoot = "Public/images/";
        public static bool ResizeAndSave(Stream stream,int width,int height,string folderName,string fileName)
        {
            try
            {
                MagickImage imageMagick = new MagickImage(stream);
                imageMagick.Resize(width, height);
                FileStream fls = new FileStream(Path.Combine(imageRoot, folderName, fileName), FileMode.Create);
                imageMagick.Write(fls);
                stream.Close();
                fls.Close();
            }
            catch(IOException)
            {
                return false;
            }
            
            return true;
        }
    }
}
