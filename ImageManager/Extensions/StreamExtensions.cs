using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] Md5Hash(this Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                // making sure the stream is set to the beginning before computing the hash
                stream.Position = 0;

                byte[] hash = md5.ComputeHash(stream);

                // repositioning the stream to the beginning for future use
                stream.Position = 0;

                return hash;
            }
        }
    }
}
