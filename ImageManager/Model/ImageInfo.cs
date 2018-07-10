using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager.Model
{
    public class ImageInfo
    {
        public string MimeType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Stream Image { get; set; }
        public string FilePath { get; set; }
    }
}
