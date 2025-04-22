using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;

namespace Servidor
{
    class Program
    {
        static TcpListener? Servidor;


        static void Main(string[] args)
        {            
            byte[] bufferLectura = new byte[1024];

            Servidor = new TcpListener(IPAddress.Parse("127.0.0.1"), 10001);
            Servidor.Start();
            Console.WriteLine("Servidor: Servidor iniciado");

            TcpClient Cliente = Servidor.AcceptTcpClient();

            if (Cliente.Connected)
            {
                Console.WriteLine("Servidor: Cliente conectado");
            }

            Console.ReadLine();
        }
    }
}

