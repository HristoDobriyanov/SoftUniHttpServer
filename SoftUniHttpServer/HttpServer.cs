﻿using System;
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
        private readonly TcpListener tcpListener;
        private bool isWorking;
        private readonly RequestProcessor requestProcessor;

        public HttpServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            this.requestProcessor = new RequestProcessor();
        }

        public void Start()
        {
            this.isWorking = true;
            this.tcpListener.Start();
            Console.WriteLine("Started...");

            while (this.isWorking)
            {
                var client = this.tcpListener.AcceptTcpClient();
                requestProcessor.ProcessClientAsync(client);
            }
        }



        public void Stop()
        {
            this.isWorking = false;
        }
    }

    public class RequestProcessor
    {
        public async Task ProcessClientAsync(TcpClient client)
        {
            var buffer = new byte[10240];
            var stream = client.GetStream();
            var readLength = await stream.ReadAsync(buffer, 0, buffer.Length);
            var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(requestText);
            var responseText = File.ReadAllText("index.html");
            var responseBytes = Encoding.UTF8.GetBytes(
                "HTTP/1.0 200 OK" + Environment.NewLine +
                "Content-type: html" + Environment.NewLine +
                "Set-cookie: lang=bg" + Environment.NewLine +
                "Content-Length: " + responseText.Length + Environment.NewLine + Environment.NewLine +
                responseText);
            await stream.WriteAsync(responseBytes);
        }
    }
}