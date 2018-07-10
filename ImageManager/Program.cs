using ImageManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new PersistentService();
            var manager = new ImageManager(service);
        }
    }
}
