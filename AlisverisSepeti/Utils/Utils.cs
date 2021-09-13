using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti.Utils
{
    public static class Utils
    {

        public static String ToFileName(string fileName, int id) 
        {
            string name = Path.GetFileNameWithoutExtension(fileName).ToLower(CultureInfo.InvariantCulture);
            string extension = Path.GetExtension(fileName);
            name.Replace(' ', '_');
            string newFileName = name + "-" + id + extension;
            return newFileName;
            
        }
    }
}
