using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToBase64String(this byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        public static string ToS3ETagString(this byte[] value)
        {
            return BitConverter.ToString(value).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
