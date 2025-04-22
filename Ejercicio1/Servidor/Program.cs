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
        private static int uniqueId = 0;


        static void Main(string[] args)
        {            
            byte[] bufferLectura = new byte[1024];

            Servidor = new TcpListener(IPAddress.Parse("127.0.0.1"), 10001);
            Servidor.Start();
            Console.WriteLine("Servidor: Servidor iniciado");

            Servidor.BeginAcceptTcpClient(EndAccept, Servidor);

            Console.WriteLine("Servidor: Esperando clientes sin bloqueo");

            Console.ReadLine();
        }


        static void EndAccept(IAsyncResult ar)
        {
            TcpListener Servidor = (TcpListener)ar.AsyncState!;
            TcpClient Cliente = Servidor.EndAcceptTcpClient(ar);

            int clientId = Interlocked.Increment(ref uniqueId);

            Console.WriteLine($"Servidor: Gestionando nuevo vehículo.. {uniqueId}");

            Servidor.BeginAcceptTcpClient(EndAccept, Servidor);
        }
    }
}

