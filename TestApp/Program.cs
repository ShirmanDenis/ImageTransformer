using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] buffer = new byte[102400];

            var stream = File.Open("TestImg.png", FileMode.Open, FileAccess.Read);
            stream.Read(buffer, 0, 102400);

            Console.WriteLine("Sended");
            Console.Read();

        }
    }
}

