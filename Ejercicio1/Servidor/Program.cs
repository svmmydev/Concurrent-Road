using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;
using System.Threading.Tasks;

namespace Servidor
{
    class Program
    {
        static TcpListener? Servidor;
        private static int IdUnico = 0;


        static async Task Main(string[] args)
        {            
            byte[] bufferLectura = new byte[1024];

            Servidor = new TcpListener(IPAddress.Parse("127.0.0.1"), 10001);
            Servidor.Start();
            Console.WriteLine("Servidor: Servidor iniciado");

            while (true)
            {
                TcpClient Cliente = await Servidor.AcceptTcpClientAsync();
                _ = GestionarClienteAsync(Cliente);
            }
        }


        private static async Task GestionarClienteAsync(TcpClient cliente)
        {
            int clienteId = Interlocked.Increment(ref IdUnico);
            Console.WriteLine($"Servidor: Gestionando nuevo vehículo #{clienteId}");

            using NetworkStream netwS = cliente.GetStream();
        }
    }
}

