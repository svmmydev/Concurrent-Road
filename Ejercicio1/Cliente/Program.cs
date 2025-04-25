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

                NetworkStream netwS = Cliente.GetStream();

                await netwS.EscribirMensajeAsync("INICIO");

                string clienteId = await netwS.LeerMensajeAsync();
                Console.WriteLine($"ID Asignado: {clienteId}");

                await netwS.EscribirMensajeAsync(clienteId);
                Console.WriteLine("Handshake completado!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"# Error al conectar con el servidor: {e.Message}");
            }

            Console.WriteLine("Presiona Enter para salir..");
            Console.ReadLine();
        }
    }
}