using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            var clock = new Stopwatch();
            var tasks = new List<Task>();

            clock.Start();

            Parallel.For(0, 30, (i) =>
            {

                    var url = "http://176.226.132.127:8080/process/sepia/1,1,1,1";
                    WebClient client = new WebClient();
                    client.UploadData(url, "POST", buffer);

            });
            
            clock.Stop();
            Console.WriteLine(clock.Elapsed);
            Console.Read();
        }
    }
}

