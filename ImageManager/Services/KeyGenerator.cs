﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager.Services
{
    public class KeyGenerator
    {
        private static readonly Random Random = new Random();

        public static string GenerateObjectKey(string fileName)
        {
            return $"{GetRandomPrefix(8)}/{DateTime.UtcNow:s}/{fileName}"
              .Replace(':', '-');
        }

        public static string GenerateObjectKeyWithSize(string originalKey, int? width, int? height, string versionId)
        {
            if (!width.HasValue)
                width = 0;

            if (!height.HasValue)
                height = 0;

            var segments = originalKey.Split('/').ToList();
            segments.Insert(2, $"{versionId}/{width}x{height}");

            return string.Join("/", segments);
        }

        private static string GetRandomPrefix(int length)
        {
            string[] result = new string[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = Random.Next(16).ToString("X");
            }

            return string.Concat(result);
        }
    }
}
