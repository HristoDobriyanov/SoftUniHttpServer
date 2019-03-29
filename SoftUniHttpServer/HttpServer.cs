using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoftUniHttpServer
{
    public class HttpServer : IHttpServer
    {
        private TcpListener tcpListener;
        private bool isWorking;

        public HttpServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
        }

        public void Start()
        {
            this.isWorking = true;
            this.tcpListener.Start();
            Console.WriteLine("Started...");

            while (this.isWorking)
            {
                var client = this.tcpListener.AcceptTcpClient();
                Task.Run(() => ProcessClientAsync(client));

            }

        }

        private static async void ProcessClientAsync(TcpClient client)
        {
            var buffer = new byte[10240];
            var stream = client.GetStream();
            var readLength = await stream.ReadAsync(buffer, 0, buffer.Length);
            var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(requestText);
            await Task.Run(() => Thread.Sleep(10000));
            var responseText = System.DateTime.Now.ToString(); ////File.ReadAllText("index.html");

            var responseBytes = Encoding.UTF8.GetBytes(
                "HTTP/1.0 200 OK" + Environment.NewLine +
                "Content-Length: " + responseText.Length + Environment.NewLine + Environment.NewLine +
                responseText);

            await stream.WriteAsync(responseBytes);
        }

        public void Stop()
        {
            this.isWorking = false;
        }
    }
}