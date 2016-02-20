using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static bool isAtEnd(byte[] bytes) {
            int L = bytes.Length;
            return bytes[L - 1] == '\n' && bytes[L - 2] == '\r' && bytes[L - 3] == '\n';
        }
        static void Main(string[] args)
        {
            TcpListener tcplistener = null;
            IPAddress localhost = Dns.GetHostEntry("localhost").AddressList[0];
            try {
                tcplistener = new TcpListener(localhost, 8080);
                tcplistener.Start();
            } catch (Exception e) {
                Console.WriteLine("Error:" + e);
            }
            
            byte[] outputBuffer = System.Text.Encoding.ASCII.GetBytes("<html><body><h1>Hello World!</h1></body</html>");

            while (true) {
                Thread.Sleep(10); //Msdn says so. Think it's wrong.
                TcpClient tcpClient = tcplistener.AcceptTcpClient();
                byte[] bytes = new byte[256];
                NetworkStream stream = tcpClient.GetStream();

                int bytesRead = 0;
                do {
                    bytesRead = stream.Read(bytes, 0, bytes.Length);
                    Console.Write(System.Text.Encoding.Default.GetString(bytes));
                } while (bytesRead > 0 && isAtEnd(bytes)); //Screwed if we're malformed.
                stream.Write(outputBuffer, 0, outputBuffer.Length);
            }
        }
    }
}
