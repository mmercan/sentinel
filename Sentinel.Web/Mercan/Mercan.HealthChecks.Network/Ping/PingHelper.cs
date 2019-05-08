using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Mercan.HealthChecks.Network.Ping
{
    public class PingHelper
    {

        public string TcpPing(string endPoint)
        {
            if (endPoint.Split(':').Count() != 2)
            {
                throw new Exception("Please use dns:port format");
            }
            var dns = endPoint.Split(':')[0];
            var port = Convert.ToInt32(endPoint.Split(':')[1]);
            var times = new List<double>();
            for (int i = 0; i < 4; i++)
            {
                var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Blocking = true;
                var stopwatch = new Stopwatch();
                // Measure the Connect call only
                stopwatch.Start();
                var dnsEndPoint = new System.Net.DnsEndPoint(dns, port);
                sock.Connect(dnsEndPoint);
                stopwatch.Stop();
                double t = stopwatch.Elapsed.TotalMilliseconds;
                Console.WriteLine("{0:0.00}ms", t);
                times.Add(t);
                sock.Close();
                Thread.Sleep(1000);
            }
            return string.Format("{0:0.00} {1:0.00} {2:0.00}", times.Min(), times.Max(), times.Average());
        }

    }
}
