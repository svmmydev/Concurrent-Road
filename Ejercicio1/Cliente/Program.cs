using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using NetworkStreamNS;
using CarreteraClass;
using VehiculoClass;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static TcpClient? Cliente;

        static async Task Main(string[] args)
        {
            try
            {
                using TcpClient Cliente = new TcpClient();
                await Cliente.ConnectAsync("127.0.0.1", 10001);
                
                Console.WriteLine("Cliente: Conectado al servidor");

                // TODO Coger el stream e implementar el handshake
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al conectar con el servidor: {e}");
            }

            Console.ReadLine();
        }
    }
}